using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<Producto?> ObtenerPorIdParaActualizarAsync(int id);
        Task<Producto?> ObtenerPorIdConDetallseAsync(int id);
        Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids);
        Task AgregarAsync(Producto producto);
        void Eliminar(Producto producto);
    }
}