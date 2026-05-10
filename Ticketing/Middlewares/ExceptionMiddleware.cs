using System.Data;
using Ticketing.Domain.Exceptions;
using FluentValidation;

namespace Ticketing.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("[CODE-ERROR] - VAL-001: Error de validación en {Path}: {Message}",
                    context.Request.Path, ex.Message);

                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                await HandleException(context, StatusCodes.Status400BadRequest,
                    "[CODE-ERROR] - VAL-001: Error de validación de datos.", errors);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("[CODE-ERROR] - NOT-002: Recurso no encontrado en {Path}: {Message}",
                    context.Request.Path, ex.Message);

                await HandleException(context, 404,
                    $"[CODE-ERROR] - NOT-002: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("[CODE-ERROR] - OPS-003: Operación inválida en {Path}: {Message}",
                    context.Request.Path, ex.Message);

                await HandleException(context, 409,
                    $"[CODE-ERROR] - OPS-003: {ex.Message}");
            }
            catch (DBConcurrencyException ex)
            {
                _logger.LogWarning("[CODE-ERROR] - CON-004: Conflicto de concurrencia DB en {Path}: {Message}",
                    context.Request.Path, ex.Message);

                await HandleException(context, 409,
                    $"[CODE-ERROR] - CON-004: {ex.Message}");
            }
            catch (ConcurrencyConflictException ex)
            {
                _logger.LogWarning("[CODE-ERROR] - CON-005: Conflicto de concurrencia en {Path}: {Message}",
                    context.Request.Path, ex.Message);

                await HandleException(context, 409,
                    $"[CODE-ERROR] - CON-005: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CODE-ERROR] - SRV-500: Error interno no controlado en {Path}",
                    context.Request.Path);

                await HandleException(context, StatusCodes.Status500InternalServerError,
                    "[CODE-ERROR] - SRV-500: Error interno del servidor.");
            }
        }

        private static async Task HandleException(HttpContext context, int statusCode, string message, object details = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = statusCode,
                message,
                details
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}