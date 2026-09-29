using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteResponse>> ObtenerTodosAsync();
        Task<ClienteResponse?> ObtenerPorIdAsync(int id);
        Task<ClienteResponse> CrearAsync(ClienteRequest request);
        Task<ClienteResponse> ActualizarAsync(int id, ClienteRequest request);
        Task<ClienteResponse> ElimminarAsync(int id);
    }
}






