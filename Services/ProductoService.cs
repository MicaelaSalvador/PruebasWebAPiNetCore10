using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Services
{
  public class ProductoService
  {
    public List<Producto> Productos { get; } = new()
        {
          new Producto
          {
            Id = 1,
            Nombre = "Laptop",
            Precio = 15000,
            Stock =10
          },
        new Producto
        {
            Id = 2,
            Nombre = "Mouse",
            Precio =500,
            Stock =25
        },
        new Producto
        {
           Id = 3,
           Nombre = "Teclado",
          Precio = 800,
          Stock =15
        }
    };
  }
}

