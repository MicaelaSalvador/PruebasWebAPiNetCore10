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
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;
        public ProductoRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<Producto>();

            return await _context.Productos
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
        }
    }
}




















