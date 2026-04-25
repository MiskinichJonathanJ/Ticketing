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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Seat)
                .WithMany()
                .HasForeignKey(r => r.SeatId)
                .IsRequired();

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .IsRequired(false); 
        }
    }
}
