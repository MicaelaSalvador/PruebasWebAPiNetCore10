using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SistemaVentasAPI.DTOs;

namespace SistemaVentasAPI.Validators
{
    public class ProductoRequestValidator : AbstractValidator<ProductoRequest>
    {
        public ProductoRequestValidator()
        {
            RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MinimumLength(3)
            .WithMessage("El nombre debe tener al menos 3 caracteres")
            .MaximumLength(100)
            .WithMessage("El nombre no debe superar los 100 caracteres");

            RuleFor(x => x.Precio)
            .GreaterThan(0)
            .WithMessage("El precio debe ser mayor que 0.");

            RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo");
        }
    }
}
















































