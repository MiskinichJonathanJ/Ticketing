using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
        {
        }

        // Aquí puedes agregar tus DbSet<Entity> una vez que definas tus entidades en Ticketing.Domain
        // public DbSet<YourEntity> YourEntities { get; set; }
        public DbSet<Event> EVENT { get; set; }
        public DbSet<Sector> SECTOR { get; set; }
        public DbSet<Seat> SEAT { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
                entity.Property(e => e.EventDate)
                .IsRequired();

                //Relacion con Sector
                entity.HasMany(e => e.Sectors)
                    .WithOne(s => s.Event)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                // ... resto de la config
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(s => s.Price)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                entity.Property(s => s.Capacity)
                    .IsRequired();

                // Relación con Seat
                entity.HasMany(s => s.Seats)
                    .WithOne(se => se.Sector)
                    .HasForeignKey(se => se.SectorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(s => s.RowIdentifier)
                    .IsRequired()
                    .HasColumnType("varchar(10)");

                entity.Property(s => s.SeatNumber)
                    .IsRequired();

                entity.Property(s => s.Status)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                // Optimistic Locking
                entity.Property(s => s.Version)
                    .IsConcurrencyToken();

                // Relación 1:1 con Reservation
                //entity.HasOne(s => s.Reservation)
                //    .WithOne(r => r.Seat)
                 //   .HasForeignKey<Reservation>(r => r.SeatId)
                 //   .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
