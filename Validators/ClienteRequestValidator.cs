using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SistemaVentasAPI.DTOs;

namespace SistemaVentasAPI.Validators
{
    public class ClienteRequestValidator : AbstractValidator<ClienteRequest>
    {
        public ClienteRequestValidator()
        {
            RuleFor(x => x.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres")
            .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("El email no tiene un formato válido.");
        }
    }
}
























