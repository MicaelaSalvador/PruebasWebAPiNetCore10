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
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClientesController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponse>>> GetClientes()
        {
            var clientes = await _context.Clientes
            .AsNoTracking()

            .Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Email = c.Email
            })
            .ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponse>> GetCliente(int id)
        {
            var cliente = await _context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Email = c.Email
            })
            .FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponse>> CrearCliente(ClienteRequest request)
        {
            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                Email = request.Email
            };
            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            var response = new ClienteResponse
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };

            return CreatedAtAction(
                nameof(GetCliente),
                new { id = cliente.Id },
           response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteResponse>> ActualizarCliente(int id, ClienteRequest request)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            cliente.Nombre = request.Nombre;
            cliente.Email = request.Email;
            await _context.SaveChangesAsync();

            var response = new ClienteResponse
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ClienteResponse>> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            var response = new ClienteResponse
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email
            };
            return response;
        }
    }
}






























