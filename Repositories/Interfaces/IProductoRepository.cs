using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerPorIdsAsync(List<int> ids);
    }
}