using Microsoft.EntityFrameworkCore;
using Ticketing.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var context = services.GetRequiredService<TicketingDbContext>();
            logger.LogInformation("Iniciando migración de base de datos de forma asíncrona...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Migración completada con éxito.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Error fatal durante la migración o seeding de la base de datos. La aplicación se detendrá.");
            throw;
        }
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();