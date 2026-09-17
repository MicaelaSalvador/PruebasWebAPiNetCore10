using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentasAPI.DTOs
{
    public class PedidoResponse
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<DetallePedidoResponse> Detalles { get; set; } = new();
    }

    public class DetallePedidoResponse
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}