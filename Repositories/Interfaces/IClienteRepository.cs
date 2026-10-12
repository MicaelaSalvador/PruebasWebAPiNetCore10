using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.DTOs.Pagination;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ObtenerTodosAsync();
        Task<Cliente?> ObtenerPorIdAsync(int id);
        Task<Cliente?> ObtenerPorIdParaActualizarAsync(int id);
        Task<Cliente?> ObtenerPorIdConPedidoAsync(int id);
        Task AgregarAsync(Cliente cliente);
        void Eliminar(Cliente cliente);
        Task<(List<Cliente> Items, int TotalItems)> ObtenerPaginadosAsync(ClienteQueryParameters parameters);
    }
}










