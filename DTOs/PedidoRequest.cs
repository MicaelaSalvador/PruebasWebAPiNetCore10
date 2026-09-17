using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentasAPI.DTOs
{
    public class PedidoRequest
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "El pedido debe tener al menos un producto")]
        public List<DetallePedidoRequest> Detalles { get; set; } = new();
    }

    public class DetallePedidoRequest
    {
        [Required]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}