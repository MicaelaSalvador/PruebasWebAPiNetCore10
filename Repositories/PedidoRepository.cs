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
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<Pedido?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pedidos
            // .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);
            // 👆 Quité AsNoTracking porque puede que quieras modificar el pedido después
        }
        public async Task AgregarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        /*  public async Task GuardarCambiosAsync()
         {
             await _context.SaveChangesAsync();
         } */

    }
}







