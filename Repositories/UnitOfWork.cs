using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaVentasAPI.Data;
using SistemaVentasAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace SistemaVentasAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public IClienteRepository Clientes { get; }
        public IProductoRepository Productos { get; }
        public IPedidoRepository Pedidos { get; }

        public UnitOfWork(
            AppDbContext context,
            IClienteRepository clienteRepository,
            IProductoRepository productoRepository,
            IPedidoRepository pedidoRepository)
        {
            _context = context;
            Clientes = clienteRepository;
            Productos = productoRepository;
            Pedidos = pedidoRepository;
        }

        public async Task<int> GuardarCambiosAsync()
        => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
        => _transaction = await _context.Database.BeginTransactionAsync();

        public async Task CommitAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}














