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
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(p => p.Nombre)
               .HasMaxLength(100)
               .IsRequired();

                entity.Property(p => p.Precio)
               .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(c => c.Nombre)
                .HasMaxLength(150)
                .IsRequired();

                entity.Property(c => c.Email)
                .HasMaxLength(150)
                .IsRequired();
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DetallePedido>(entity =>
            {
                entity.Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

                entity.HasOne(d => d.Pedido)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.PedidoId);

                entity.HasOne(d => d.Producto)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}















