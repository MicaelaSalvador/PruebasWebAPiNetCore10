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
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteService> _logger;
        public ClienteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClienteService> logger)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ClienteResponse>> ObtenerTodosAsync()
        {
            var clientes = await _unitOfWork.Clientes.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ClienteResponse>>(clientes);
        }
        public async Task<ClienteResponse?> ObtenerPorIdAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.ObtenerPorIdAsync(id);

            if (cliente == null)
            {
                _logger.LogWarning("Cliente no encontrado. Id: {Id}", id);
                return null;
            }

            return _mapper.Map<ClienteResponse>(cliente);
        }
        public async Task<ClienteResponse> CrearAsync(ClienteRequest request)
        {
            var cliente = _mapper.Map<Cliente>(request);

            await _unitOfWork.Clientes.AgregarAsync(cliente);

            await _unitOfWork.GuardarCambiosAsync();

            _logger.LogInformation("Cliente creado. Id: {id},Nombre :{Nombre}", cliente.Id, cliente.Nombre);

            return _mapper.Map<ClienteResponse>(cliente);
        }
        public async Task<ClienteResponse> ActualizarAsync(int id, ClienteRequest request)
        {
            var cliente = await _unitOfWork.Clientes.ObtenerPorIdParaActualizarAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente no encontrado para actualizar. Id: {id}", id);
                return null;
            }
            _mapper.Map(request, cliente);
            await _unitOfWork.GuardarCambiosAsync();

            _logger.LogInformation("Cliente actualizado . Id : {id}, Nombre: {Nombre}", cliente.Id, cliente.Nombre);

            return _mapper.Map<ClienteResponse>(cliente);
        }
        public async Task<ClienteResponse> ElimminarAsync(int id)
        {
            var cliente = await _unitOfWork.Clientes.ObtenerPorIdConPedidoAsync(id);

            if (cliente == null)
            {
                _logger.LogWarning("Cliente no encontrado para eliminar. Id : {Id}", id);
                return null;
            }
            if (cliente.Pedidos.Any())
            {
                _logger.LogWarning("Intento de eliminar cliente con pedidos asociados. Id: {Id}", id);
                throw new BusinessException("No se puede eliminar el cleinte por que tiene pedidos asociados");
            }
            var response = _mapper.Map<ClienteResponse>(cliente);
            _unitOfWork.Clientes.Eliminar(cliente);
            await _unitOfWork.GuardarCambiosAsync();
            _logger.LogInformation("Cliente eliminado. Id :{id}, Nombre :{Nombre}", cliente.Id, cliente.Nombre);

            return response;
        }
    }
}







