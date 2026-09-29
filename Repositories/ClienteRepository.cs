using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Repositories.Interfaces;

namespace SistemaVentasAPI.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;
        public ClienteRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _context.Clientes
            .AsNoTracking()
            .ToListAsync();
        }
        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Cliente?> ObtenerPorIdParaActualizarAsync(int id)
        {
            return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente?> ObtenerPorIdConPedidoAsync(int id)
        {
            return await _context.Clientes
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AgregarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        public void Eliminar(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }
    }
}
















