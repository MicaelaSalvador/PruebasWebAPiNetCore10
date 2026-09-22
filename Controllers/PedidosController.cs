using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Exceptions;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Services;
using SistemaVentasAPI.Services.Interfaces;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {



        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            this._pedidoService = pedidoService;

        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponse>> CrearPedido(PedidoRequest request)
        {
            try
            {
                var respose = await _pedidoService.CrearPedidoAsync(request);
                return CreatedAtAction(
                    nameof(GetPedido),
                    new { id = respose.Id },
                    respose);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponse>> GetPedido(int id)
        {
            var pedido = await _pedidoService.ObtenerPedidoAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }
    }
}























