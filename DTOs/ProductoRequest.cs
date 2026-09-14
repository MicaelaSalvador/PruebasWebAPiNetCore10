using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentasAPI.DTOs
{
    public class ProductoRequest
    {
        [Required(ErrorMessage = "El nombre es obligatoria")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener  entre 3 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        [Range(0.01, 999999999, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }
}








