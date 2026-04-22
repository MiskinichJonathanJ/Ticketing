using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Data
{
    public class TicketingDbContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedEvents(modelBuilder);
            SeedSectors(modelBuilder);
            SeedSeats(modelBuilder);
        }

        private static void SeedEvents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(new Event
            {
                Id = 1,
                Name = "Conferencia de Inteligencia Artificial 2026",
                EventDate = new DateTime(2026, 6, 23),
                Venue = "Auditorio Tecnológico - Sala Principal",
                Status = "Active"
            });
        }

        private static void SeedSectors(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sector>().HasData(
                new Sector { Id = 1, EventId = 1, Name = "Campo General", Price = 95000, Capacity = 50 },
                new Sector { Id = 2, EventId = 1, Name = "Platea ", Price = 120000, Capacity = 50 }
            );
        }

        private static void SeedSeats(ModelBuilder modelBuilder)
        {
            var seats = new List<Seat>();
            int index = 1;

            for (int sectorId = 1; sectorId <= 2; sectorId++)
            {
                for (int i = 1; i <= 50; i++)
                {
                    seats.Add(new Seat
                    {
                        Id = GenerateSeatGuid(sectorId, index),
                        SectorId = sectorId,
                        RowIdentifier = GetRowIdentifier(i),
                        SeatNumber = i,
                        Status = "Available",
                        Version = 0
                    });
                    index++;
                }
            }

            modelBuilder.Entity<Seat>().HasData(seats);
        }

        // GUID fijo para evitar migraciones innecesarias
        private static Guid GenerateSeatGuid(int sectorId, int index) =>
            Guid.Parse($"a{sectorId:D7}-0000-0000-0000-{index:D12}");

        // 10 butacas por fila → F1 a F5
        private static string GetRowIdentifier(int seatNumber) =>
            $"F{(seatNumber - 1) / 10 + 1}";
    }
}
