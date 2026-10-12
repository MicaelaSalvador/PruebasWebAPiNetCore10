using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.DTOs.Pagination;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponse>> ObtenerTodosAsync();
        Task<ProductoResponse?> ObtenerPorIdAsync(int id);
        Task<ProductoResponse> CrearAsync(ProductoRequest request);
        Task<ProductoResponse?> ActualizarAsync(int id, ProductoRequest request);
        Task<ProductoResponse?> EliminarAsync(int id);
        Task<PagedResponse<ProductoResponse>> ObtenerPaginadosAsync(ProductoQueryParameters parameters);
    }
}













