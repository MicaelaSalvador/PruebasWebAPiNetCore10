using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SistemaVentasAPI.DTOs;
using SistemaVentasAPI.Exceptions;
using SistemaVentasAPI.Models;
using SistemaVentasAPI.Repositories.Interfaces;
using SistemaVentasAPI.Services.Interfaces;

namespace SistemaVentasAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductoService> _logger;

        public ProductoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductoService> logger)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;

        }
        public async Task<IEnumerable<ProductoResponse>> ObtenerTodosAsync()
        {
            var productos = await _unitOfWork.Productos.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ProductoResponse>>(productos);
        }

        public async Task<ProductoResponse?> ObtenerPorIdAsync(int id)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado. Id: {id}", id);
                return null;
            }
            return _mapper.Map<ProductoResponse>(producto);
        }
        public async Task<ProductoResponse> CrearAsync(ProductoRequest request)
        {
            var producto = _mapper.Map<Producto>(request);

            await _unitOfWork.Productos.AgregarAsync(producto);
            await _unitOfWork.GuardarCambiosAsync();

            return _mapper.Map<ProductoResponse>(producto);
        }
        public async Task<ProductoResponse?> ActualizarAsync(int id, ProductoRequest request)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdParaActualizarAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado para actualizar. Id: {Id}", id);
                return null;
            }
            _mapper.Map(request, producto);
            await _unitOfWork.GuardarCambiosAsync();
            _logger.LogInformation("Producto actualizado. Id : {id}, Nombre : {Nombre}", producto.Id, producto.Nombre);
            return _mapper.Map<ProductoResponse>(producto);
        }
        public async Task<ProductoResponse?> EliminarAsync(int id)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdConDetallseAsync(id);
            if (producto == null)
            {
                _logger.LogWarning("Producto no encontrado para eliminar. Id :{Id}", id);
                return null;
            }

            if (producto.Detalles.Any())
            {
                _logger.LogWarning("Intento de eliminar producto con pedidos asociados. Id {Id]}", id);
                throw new BusinessException("No se puede eliminar el producto por que  tiene pedidos asociados");
            }

            var response = _mapper.Map<ProductoResponse>(producto);

            _unitOfWork.Productos.Eliminar(producto);
            await _unitOfWork.GuardarCambiosAsync();

            _logger.LogInformation("Producto eliminado. Id: {Id}, Nombre: {Nombre}", producto.Id, producto.Nombre);

            return response;

        }
    }
}



































