using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {

        private readonly AppDbContext _context;
        public ProductosController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos()
        {
            var productos = await _context.Productos
            .AsNoTracking()
            .ToListAsync();

            var respose = productos.Select(p => new ProductoResponse
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            });
            return Ok(respose);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetProducto(int id)
        {
            var producto = await _context.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            var response = new ProductoResponse
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoResponse>> CrearProducto(ProductoRequest request)
        {
            var producto = new Producto
            {
                Nombre = request.Nombre,
                Precio = request.Precio,
                Stock = request.Stock
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var response = new ProductoResponse
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock
            };

            return CreatedAtAction(
                nameof(GetProducto),
                new { id = producto.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoResponse>> ActualizarProducto(int id, ProductoRequest request)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            producto.Nombre = request.Nombre;
            producto.Precio = request.Precio;
            producto.Stock = request.Stock;

            await _context.SaveChangesAsync();

            var response = new ProductoResponse
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}


















