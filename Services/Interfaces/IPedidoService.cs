using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.DTOs;


namespace SistemaVentasAPI.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CrearPedidoAsync(PedidoRequest request);

        Task<PedidoResponse?> ObtenerPedidoAsync(int id);
    }
}