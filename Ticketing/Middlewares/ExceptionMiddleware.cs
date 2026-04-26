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
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, 404, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await HandleException(context, 409, ex.Message);
            }
            catch (Exception)
            {
                await HandleException(context, 500, "Internal server error");
            }
        }

        private static async Task HandleException(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
