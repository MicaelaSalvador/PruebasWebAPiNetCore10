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
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
            .AsNoTracking()
            .ToListAsync();
        }
        public async Task<Producto?> ObtenerPorIdParaActualizarAsync(int id)
        {
            return await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto?> ObtenerPorIdConDetallseAsync(int id)
        {
            return await _context.Productos
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<Producto>();

            return await _context.Productos
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
        }
        public async Task AgregarAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
        }

        public void Eliminar(Producto producto)
        {
            _context.Productos.Remove(producto);
        }

    }
}




















