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

        public async Task<(List<Producto> Items, int TotalItems)> ObtenerPaginadosAsync(ProductoQueryParameters parameters)
        {
            var query = _context.Productos.AsNoTracking().AsQueryable();

            //Filtros: Search(nombre)
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                query = query.Where(p => p.Nombre.ToLower().Contains(search));
            }

            // Filtro: PrecioM
            if (parameters.PrecioMin.HasValue)
                query = query.Where(p => p.Precio >= parameters.PrecioMin.Value);

            // Filtro : PrecioMax
            if (parameters.PrecioMax.HasValue)
                query = query.Where(p => p.Precio <= parameters.PrecioMax.Value);

            // Total Antes de paginar
            var totalItems = await query.CountAsync();

            // Ordenamiento
            var SortBy = parameters.SortBy.ToLower();
            var SortOrder = parameters.SortOrder.ToLower();

            query = SortBy switch
            {
                "precio" => SortOrder == "desc"
                ? query.OrderByDescending(p => p.Precio)
                : query.OrderBy(p => p.Precio),

                "stock" => SortOrder == "desc"
                ? query.OrderByDescending(p => p.Stock)
                : query.OrderBy(p => p.Stock),

                _ => SortOrder == "desc"
                ? query.OrderByDescending(p => p.Nombre)
                : query.OrderBy(p => p.Nombre)
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





























