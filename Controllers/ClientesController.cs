using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.DTOs.Pagination;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Services.Interfaces;

namespace SistemaVentasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClientesController(IClienteService clienteService)
        {
            this._clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ClienteResponse>>> GetClientes([FromQuery] ClienteQueryParameters parameters)
        {
            var response = await _clienteService.ObtenerPaginadosAsync(parameters);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> GetCliente(int id)
        {
            var response = await _clienteService.ObtenerPorIdAsync(id);
            if (response == null)
            {
                return NotFound(new { mensaje = $"No se encontró el clienete con el Id : {id}" });
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> CrearCliente(ClienteRequest request)
        {
            var response = await _clienteService.CrearAsync(request);
            return CreatedAtAction(
              nameof(GetCliente),
              new { id = response.Id },
              response);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponse>> ActualizarCliente(int id, ClienteRequest request)
        {
            var response = await _clienteService.ActualizarAsync(id, request);
            if (response == null)
                return NotFound(new { mesaje = $"No se encontró el cliente con Id {id}" });
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteResponse>> EliminarCliente(int id)
        {
            var response = await _clienteService.ElimminarAsync(id);
            if (response == null)
                return NotFound(new { mensaje = $"No se encontró el cliente con el Id {id}" });
            return Ok(response);
        }
    }
}






























