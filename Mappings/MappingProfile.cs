using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Producto, ProductoResponse>();
            CreateMap<ProductoRequest, Producto>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Detalles, opt => opt.Ignore());
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<ClienteRequest, Cliente>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Pedidos, opt => opt.Ignore());

        }
    }
}












