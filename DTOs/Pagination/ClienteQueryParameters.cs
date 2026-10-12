using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVentasAPI.DTOs.Pagination
{
    public class ClienteQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; } // busca en Nombre y Email
        public string SortBy { get; set; } = "nombre";
        public string SortOrder { get; set; } = "asc";
    }
}










