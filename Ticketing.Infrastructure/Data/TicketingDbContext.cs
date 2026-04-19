using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Seat> Seats { get; set; } 

        // TODO: Considerar añadir DbSets para Event y Sector una vez que se creen sus entidades correspondientes en Ticketing.Domain.Entities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciòn para User
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .IsRequired(false); // UserId es opcional en AuditLog

            // Configuraciòn para Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Seat)
                .WithMany() // Asumimos que un Seat puede tener muchas Reservations históricamente, pero solo una activa
                .HasForeignKey(r => r.SeatId)
                .IsRequired();

            // Configuraciòn para AuditLog
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .IsRequired(false); // UserId es opcional

            // Configuraciòn para Seat (si fuera necesario, aquí iría la configuraciòn de Seat una vez creada)
            // Por ejemplo, si Seat tuviera una relaciòn con Sector:
            // modelBuilder.Entity<Seat>()
            //     .HasOne(s => s.Sector)
            //     .WithMany(s => s.Seats)
            //     .HasForeignKey(s => s.SectorId);
        }
    }
}
