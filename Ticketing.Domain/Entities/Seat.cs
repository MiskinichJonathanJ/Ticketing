using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public SeatStatus Status { get; set; };
        public int Version { get; set; } = 0;

        public Sector Sector { get; set; } = null!;

        public Reservation? Reservation { get; set; }


    }
}
