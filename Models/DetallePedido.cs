using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentasAPI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null;
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}