using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SistemaVentasAPI.DTOs.Pagination;

namespace SistemaVentasAPI.Validators
{
    public class ProductoQueryParametersValidator : AbstractValidator<ProductoQueryParameters>
    {
        private static readonly string[] SortFieldsPermitidos = { "nombre", "precio", "stock" };
        private static readonly string[] SortOrdersPermitidos = { "asc", "desc" };

        public ProductoQueryParametersValidator()
        {
            RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("La página debe ser mayor o igual a 1 .");

            RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("El tamaño de la página debe estar entre 1 y 100 ");

            RuleFor(x => x.PrecioMin)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PrecioMin.HasValue)
            .WithMessage("El precio minimo no puede ser negativo");

            RuleFor(x => x.PrecioMax)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PrecioMax.HasValue)
            .WithMessage("El precio máximo no puede ser negativo");

            RuleFor(x => x)
            .Must(x => !x.PrecioMin.HasValue || !x.PrecioMax.HasValue || x.PrecioMin <= x.PrecioMax)
            .WithMessage("El precio minimo no puede ser mayor que el máximo");

            RuleFor(x => x.SortBy)
            .Must(s => SortFieldsPermitidos.Contains(s.ToLower()))
            .WithMessage($"SortBy de ser uno de :{string.Join(",", SortFieldsPermitidos)}");

            RuleFor(x => x.SortOrder)
            .Must(s => SortOrdersPermitidos.Contains(s.ToLower()))
            .WithMessage("SortOrder de ser 'asc' o 'desc'.");

        }
    }
}





