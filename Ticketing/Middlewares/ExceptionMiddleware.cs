using System.Data;
using Ticketing.Domain.Exceptions;
using FluentValidation;

namespace Ticketing.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                await HandleException(context, StatusCodes.Status400BadRequest, "Error de validación de datos.", errors);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, 404, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandleException(context, 409, ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                await HandleException(context, 409, ex.Message);
            }
            catch (ConcurrencyConflictException ex)
            {
                await HandleException(context, 409, ex.Message);
            }
            catch (Exception)
            {
                await HandleException(context, StatusCodes.Status500InternalServerError, "Error interno del servidor.");
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
