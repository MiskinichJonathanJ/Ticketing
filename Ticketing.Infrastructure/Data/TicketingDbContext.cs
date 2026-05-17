using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
        {
        }
        public DbSet<Event> EVENT { get; set; }
        public DbSet<Sector> SECTOR { get; set; }
        public DbSet<Seat> SEAT { get; set; }
        public DbSet<User> USER { get; set; }
        public DbSet<Reservation> RESERVATION { get; set; }
        public DbSet<AuditLog> AUDIT_LOG { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Ticketing.Infrastructure.Data.TicketingDbContextSeed.Seed(modelBuilder);

            // Configuración adicional del modelo si es necesaria
            

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");
                entity.Property(e => e.Venue)
                .IsRequired()
                .HasColumnType("varchar(150)");
                entity.Property(e => e.Status)
                .IsRequired()
                .HasColumnType("varchar(20)");
                

                //Relacion con Sector
                entity.HasMany(e => e.Sectors)
                    .WithOne(s => s.Event)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
                
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
                entity.Property(s => s.Price)
                    .HasColumnType("decimal(10,2)");
;

                // Relación con Seat
                entity.HasMany(s => s.Seats)
                    .WithOne(se => se.Sector)
                    .HasForeignKey(se => se.SectorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.RowIdentifier)
                    .IsRequired()
                    .HasColumnType("varchar(10)");
                entity.Property(s => s.Status)
                    .IsRequired()
                    .HasColumnType("varchar(20)");                
                entity.Property(s => s.Version)
                    .IsConcurrencyToken();

                // Relación con Reservation: Cambiar de One-to-One a One-to-Many
               entity.HasMany(s => s.Reservations) // Un asiento puede tener muchas reservas
                     .WithOne(r => r.Seat) // Cada reserva tiene un asiento
                     .HasForeignKey(r => r.SeatId) // SeatId es la FK en Reservation
                     .IsRequired() // La FK es requerida
                     .OnDelete(DeleteBehavior.Restrict); // Evitar borrado en cascada para reservas, es mejor gestionarlo manualmente si un asiento se elimina.
            });

            // Configuracion para User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasColumnType("varchar(150)");
                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasColumnType("varchar(255)");
                
                entity.HasMany(u => u.Reservations)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.AuditLogs)
                    .WithOne(a => a.User)
                    .HasForeignKey(a => a.UserId)
                    .IsRequired(false) // UserId es opcional en AuditLog
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Status)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                // Explicitly define the many-to-one relationship from Reservation to Seat
                entity.HasOne(r => r.Seat)
                      .WithMany(s => s.Reservations) // A reservation has one seat, a seat has many reservations
                      .HasForeignKey(r => r.SeatId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Action)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                entity.Property(a => a.EntityType)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                entity.Property(a => a.EntityId)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                entity.Property(a => a.Details)
                    .HasColumnType("varchar(max)");
                entity.Property(a => a.UserId)
                    .IsRequired(false);
                entity.Property(a => a.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");
            });


        }
    }
}
