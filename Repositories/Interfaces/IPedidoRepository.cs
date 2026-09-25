using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Pedido pedido);
        //Task GuardarCambiosAsync();
    }
}