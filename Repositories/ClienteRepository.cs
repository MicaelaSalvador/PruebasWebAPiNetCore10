using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.DTOs.Pagination;
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

        public async Task<(List<Cliente> Items, int TotalItems)> ObtenerPaginadosAsync(ClienteQueryParameters parameters)
        {
            parameters ??= new ClienteQueryParameters();
            var query = _context.Clientes.AsNoTracking().AsQueryable();

            //Filtro : Search (Nombre y Email)
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                query = query.Where(c =>
                c.Nombre.ToLower().Contains(search) ||

                c.Email.ToLower().Contains(search));
            }

            //Total ANTES de paginar
            var totalItems = await query.CountAsync();

            // Ordenamiento
            var sortBy = parameters.SortBy.ToLower();
            var sortOrder = parameters.SortOrder.ToLower();

            query = sortBy switch
            {
                "email" => sortOrder == "desc"
                ? query.OrderByDescending(c => c.Email)
                : query.OrderBy(c => c.Email),

                _ => sortOrder == "desc" // default nombre
                ? query.OrderByDescending(c => c.Nombre)
                : query.OrderBy(c => c.Nombre)
            };

            // Paginación
            var items = await query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

            return (items, totalItems);
        }

    }
}



























