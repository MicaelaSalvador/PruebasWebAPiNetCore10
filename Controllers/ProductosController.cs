using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SistemaVentasAPI.Services;
using SistemaVentasAPI.Services.Interfaces;
using SistemaVentasAPI.DTOs.Pagination;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            this._productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos([FromQuery] ProductoQueryParameters parameters)
        {
            var response = await _productoService.ObtenerPaginadosAsync(parameters);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetProducto(int id)
        {
            var response = await _productoService.ObtenerPorIdAsync(id);

            if (response == null)
            {
                return NotFound(new { mensaje = $"No se emcontró el producto con Id {id}" });
            }
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ProductoResponse>> CrearProducto(ProductoRequest request)
        {
            var response = await _productoService.CrearAsync(request);
            return CreatedAtAction(
              nameof(GetProducto),
              new { id = response.Id },
              response
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoResponse>> ActualizarProducto(int id, ProductoRequest request)
        {
            var response = await _productoService.ActualizarAsync(id, request);
            if (response == null)
                return NotFound(new { mensaje = $"No se encontró el producto  con Id {id}" });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductoResponse>> EliminarProducto(int id)
        {
            var response = await _productoService.EliminarAsync(id);

            if (response == null)
                return NotFound(new { mensaje = $"No se encontró el producto con el Id {id}" });

            return response;
        }
    }
}


















