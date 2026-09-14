using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using SistemaVentasAPI.Models;

namespace SistemaVentasAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Producto> Productos => Set<Producto>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre)
               .HasMaxLength(100)
               .IsRequired();

                entity.Property(p => p.Precio)
               .HasPrecision(10, 2);
            });
        }
    }
}









