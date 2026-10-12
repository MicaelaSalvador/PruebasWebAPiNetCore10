using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace SistemaVentasAPI.DTOs.Pagination
{
    public class ClienteQueryParametersValidator : AbstractValidator<ClienteQueryParameters>
    {
        private static readonly string[] SortFieldsPermitidos = { "nombre", "email" };
        private static readonly string[] SortOrdersPermitidos = { "asc", "desc" };

        public ClienteQueryParametersValidator()
        {
            RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("La página debe ser mayor o igual a 1");

            RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("El tamaño de la página debe estar ente 1 y 100. ");

            RuleFor(x => x.SortBy)
            .Must(s => SortFieldsPermitidos.Contains(s.ToLower()))
            .WithMessage($"SortBy debe ser uno de : {string.Join(",", SortFieldsPermitidos)}");

            RuleFor(x => x.SortOrder)
            .Must(s => SortOrdersPermitidos.Contains(s.ToLower()))
            .WithMessage("SortOrder debe ser 'asc' o 'dsc'.");
        }
    }
}

















