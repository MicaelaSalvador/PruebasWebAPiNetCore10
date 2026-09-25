using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Exceptions;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Repositories.Interfaces;
using SistemaVentasAPI.Services.Interfaces;

namespace SistemaVentasAPI.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly ILogger<PedidoService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public PedidoService(IUnitOfWork unitOfWork, ILogger<PedidoService> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }
        public async Task<PedidoResponse> CrearPedidoAsync(PedidoRequest request)
        {
            // await using var transaction = await AppDbContext.Database.BeginTransactionAsync();
            _logger.LogInformation("Creando pedido por el cliente {ClienteId}", request.ClienteId);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Verificar que el cliente existe

                var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(request.ClienteId);

                if (cliente == null)
                {
                    _logger.LogWarning("Intento de crear pedido para cliente inexistente : {ClienteId}", request.ClienteId);
                    throw new BusinessException(
                        "El cliente no existe.");
                }

                // 2. Obtener los productos solicitados
                var productoIds = request.Detalles
                .Select(d => d.ProductoId)
                .Distinct()
                .ToList();

                var productos = await _unitOfWork.Productos.ObtenerPorIdsAsync(productoIds);


                // 3. Verificar que todos los productos existen.
                if (productos.Count != productoIds.Count)
                {
                    _logger.LogWarning("El pedido contiene productos inexistentes .");
                    throw new BusinessException("Uno o más productos no existen");
                }

                // 4.Verificar stock
                foreach (var detalleRequest in request.Detalles)
                {
                    var producto = productos.First(p => p.Id == detalleRequest.ProductoId);
                    if (producto.Stock < detalleRequest.Cantidad)
                    {
                        _logger.LogWarning("Stock bajo para el producto {ProductoId}: {Stock}", producto.Id, producto.Stock);
                        throw new BusinessException($"stock insuficiente para el producto: {producto.Nombre}");
                    }
                }

                //5. Crear el pedido
                var pedido = new Pedido
                {
                    Fecha = DateTime.UtcNow,
                    ClienteId = cliente.Id
                };

                //6 y7 Crear los detalles y descontar stock
                foreach (var detalleRequest in request.Detalles)
                {
                    var producto = productos.First(
                        p => p.Id == detalleRequest.ProductoId
                    );

                    pedido.Detalles.Add(new DetallePedido
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalleRequest.Cantidad,
                        PrecioUnitario = producto.Precio
                    });

                    producto.Stock -= detalleRequest.Cantidad;
                }
                // 8.Agregar el pedido
                await _unitOfWork.Pedidos.AgregarAsync(pedido);

                // 9.Guardar cambios
                await _unitOfWork.GuardarCambiosAsync();


                //10. Commit
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Pedido {PedidoId} creado correctamente", pedido.Id);

                // 11. Construir respuesta
                var total = pedido.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

                return new PedidoResponse
                {
                    Id = pedido.Id,
                    Fecha = pedido.Fecha,
                    ClienteId = cliente.Id,
                    Cliente = cliente.Nombre,
                    Total = total,
                    Detalles = pedido.Detalles
                    .Select(d =>
                    {
                        var producto = productos.First(p => p.Id == d.ProductoId);
                        return new DetallePedidoResponse
                        {
                            ProductoId = d.ProductoId,
                            Producto = producto.Nombre,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Cantidad * d.PrecioUnitario
                        };

                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear pedido para el cliente {ClienteId}", request.ClienteId);
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<PedidoResponse?> ObtenerPedidoAsync(int id)
        {
            var pedido = await _unitOfWork.Pedidos.ObtenerPorIdAsync(id);

            if (pedido == null)
            {
                return null;
            }

            return new PedidoResponse
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                ClienteId = pedido.ClienteId,
                Cliente = pedido.Cliente.Nombre,

                Total = pedido.Detalles.Sum(
                    d => d.Cantidad * d.PrecioUnitario),

                Detalles = pedido.Detalles
                    .Select(d => new DetallePedidoResponse
                    {
                        ProductoId = d.ProductoId,
                        Producto = d.Producto.Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal =
                            d.Cantidad * d.PrecioUnitario
                    })
                    .ToList()
            };

        }
    }
}





