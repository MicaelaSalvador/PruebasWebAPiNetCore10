using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SistemaVentasAPI.DTOs;

namespace SistemaVentasAPI.Validators
{
    public class PedidoRequestValidator : AbstractValidator<PedidoRequest>
    {
        public PedidoRequestValidator()
        {
            RuleFor(x => x.ClienteId)
            .GreaterThan(0)
            .WithMessage("Debe especificar un cliente válido");

            RuleFor(x => x.Detalles)
            .NotEmpty().WithMessage("El pedido debe tener al menos un producto.");

            RuleFor(x => x.Detalles)
            .Must(d => d.Select(x => x.ProductoId).Distinct().Count() == d.Count)
            .WithMessage("El pedido no puede contener productos duplicados.")
            .When(x => x.Detalles != null && x.Detalles.Any());

            RuleForEach(x => x.Detalles)
            .SetValidator(new DetallePedidoRequestValidator());
        }
    }
}















