namespace Ticketing.Middlewares
{
    public class ApiVersionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["X-Api-Version"] = "1.0";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}