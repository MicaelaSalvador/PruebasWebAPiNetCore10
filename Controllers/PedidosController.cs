using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponse>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
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
                        Subtotal = d.Cantidad * d.PrecioUnitario
                    })
                    .ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponse>> CrearPedido(
            PedidoRequest request)
        {
            // 1. Verificar que exista el cliente
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == request.ClienteId);

            if (cliente == null)
            {
                return BadRequest("El cliente no existe.");
            }

            // 2. Obtener los productos solicitados
            var productoIds = request.Detalles
                .Select(d => d.ProductoId)
                .Distinct()
                .ToList();

            var productos = await _context.Productos
                .Where(p => productoIds.Contains(p.Id))
                .ToListAsync();

            // 3. Verificar que todos los productos existan
            if (productos.Count != productoIds.Count)
            {
                return BadRequest(
                    "Uno o más productos no existen.");
            }

            // 4. Verificar stock
            foreach (var detalleRequest in request.Detalles)
            {
                var producto = productos
                    .First(p => p.Id == detalleRequest.ProductoId);

                if (producto.Stock < detalleRequest.Cantidad)
                {
                    return BadRequest(
                        $"Stock insuficiente para el producto: {producto.Nombre}");
                }
            }

            // 5. Crear el pedido
            var pedido = new Pedido
            {
                ClienteId = request.ClienteId,
                Fecha = DateTime.UtcNow
            };

            _context.Pedidos.Add(pedido);

            // 6. Crear los detalles y descontar stock
            foreach (var detalleRequest in request.Detalles)
            {
                var producto = productos
                    .First(p => p.Id == detalleRequest.ProductoId);

                var detalle = new DetallePedido
                {
                    Pedido = pedido,
                    ProductoId = producto.Id,
                    Cantidad = detalleRequest.Cantidad,
                    PrecioUnitario = producto.Precio
                };

                pedido.Detalles.Add(detalle);

                producto.Stock -= detalleRequest.Cantidad;
            }

            // 7. Guardar
            await _context.SaveChangesAsync();

            // 8. Calcular total
            var total = pedido.Detalles.Sum(
                d => d.Cantidad * d.PrecioUnitario);

            var response = new PedidoResponse
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                ClienteId = cliente.Id,
                Cliente = cliente.Nombre,
                Total = total,
                Detalles = pedido.Detalles
                    .Select(d => new DetallePedidoResponse
                    {
                        ProductoId = d.ProductoId,
                        Producto = productos
                            .First(p => p.Id == d.ProductoId)
                            .Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Cantidad * d.PrecioUnitario
                    })
                    .ToList()
            };

            return CreatedAtAction(
                nameof(GetPedido),
                new { id = pedido.Id },
                response);

        }
    }
}








