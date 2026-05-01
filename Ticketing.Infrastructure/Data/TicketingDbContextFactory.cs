using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ticketing.Infrastructure.Data
{
    public class TicketingDbContextFactory : IDesignTimeDbContextFactory<TicketingDbContext>
    {
        public TicketingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ticketing")) // Adjust path to your startup project
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TicketingDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback for design-time if connection string is not found in appsettings.json
                // This is a dummy connection string, actual connection will be used at runtime from Program.cs
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TicketingDb_Design;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionString);
            }

            return new TicketingDbContext(optionsBuilder.Options);
        }
    }
}
