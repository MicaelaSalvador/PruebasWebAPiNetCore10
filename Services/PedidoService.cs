using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Exceptions;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Services.Interfaces;

namespace SistemaVentasAPI.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            this._context = context;

        }
        public async Task<PedidoResponse> CrearPedidoAsync(PedidoRequest request)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Verificar que el cliente existe
                var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == request.ClienteId);

                if (cliente == null)
                {
                    throw new BusinessException(
                        "El cliente no existe.");
                }

                // 2. Obtener los productos solicitados
                var productoIds = request.Detalles
                .Select(d => d.ProductoId)
                .Distinct()
                .ToList();

                var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.Id))
                .ToListAsync();

                // 3. Verificar que todos los productos existen.
                if (productos.Count != productoIds.Count)
                {
                    throw new BusinessException("Uno o más productos no existen");
                }

                // 4.Verificar stock
                foreach (var detalleRequest in request.Detalles)
                {
                    var producto = productos.First(p => p.Id == detalleRequest.ProductoId);
                    if (producto.Stock < detalleRequest.Cantidad)
                    {
                        throw new BusinessException($"stock insuficiente para el producto: {producto.Nombre}");
                    }
                }

                //5. Crear el pedido
                var pedido = new Pedido
                {
                    Fecha = DateTime.UtcNow,
                    ClienteId = cliente.Id
                };

                //6.Crear los detalles
                foreach (var detalleRequest in request.Detalles)
                {
                    var producto = productos.First(
                        p => p.Id == detalleRequest.ProductoId
                    );

                    var detalle = new DetallePedido
                    {
                        ProductoId = producto.Id,
                        Cantidad = detalleRequest.Cantidad,
                        PrecioUnitario = producto.Precio
                    };
                    pedido.Detalles.Add(detalle);

                    // 7.  Descontar stock
                    producto.Stock -= detalleRequest.Cantidad;
                }
                // 8.Agregar el pedido
                _context.Pedidos.Add(pedido);

                // 9.Guardar cambios
                await _context.SaveChangesAsync();

                //10. Confirmar transaccion 
                await transaction.CommitAsync();

                // 11. Calcular total
                var total = pedido.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

                // 12. Crear respuesta
                var response = new PedidoResponse
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
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PedidoResponse?> ObtenerPedidoAsync(int id)
        {
            var pedido = await _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return null;
            }

            var response = new PedidoResponse
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

            return response;
        }
    }
}