using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SistemaVentasAPI.DTOs;


namespace SistemaVentasAPI.Validators
{
    public class DetallePedidoRequestValidator : AbstractValidator<DetallePedidoRequest>
    {
        public DetallePedidoRequestValidator()
        {
            RuleFor(x => x.ProductoId)
            .GreaterThan(0).WithMessage("Debe especificar un producto válido.");

            RuleFor(x => x.Cantidad)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor que 0.");
        }
    }
}















