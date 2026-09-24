using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using SistemaVentasAPI.Exceptions;
using System.Threading.Tasks;

namespace SistemaVentasAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                mensaje = exception.Message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                mensaje = "Ocurrió un error interno en el servidor"
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

    }
}










