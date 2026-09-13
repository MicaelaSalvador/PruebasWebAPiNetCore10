using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Services;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;
        public ProductosController(ProductoService productoService)
        {
            this._productoService = productoService;

        }

        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetProductos()
        {
            return Ok(_productoService.Productos);
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> GetProducto(int id)
        {
            var producto = _productoService.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Producto> CrearProducto(ProductoRequest request)
        {
            var producto = new Producto
            {
                Id = _productoService.Productos.Max(p => p.Id) + 1,
                Nombre = request.Nombre,
                Precio = request.Precio,
                Stock = request.Stock
            };

            _productoService.Productos.Add(producto);
            var response = new ProductoResponse
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock
            };

            return CreatedAtAction(
             nameof(GetProducto),
             new { id = producto.Id },
             response);
        }

        [HttpPut("{id}")]
        public ActionResult<Producto> ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = _productoService.Productos.FirstOrDefault(p => p.Id == id);
            if (productoExistente == null)
            {
                return NotFound();
            }
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            return Ok(productoExistente);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminaProducto(int id)
        {
            var producto = _productoService.Productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            _productoService.Productos.Remove(producto);
            return NoContent();
        }
    }
}