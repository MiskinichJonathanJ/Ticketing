using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entities
{
    public class SEAT
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public string RowIdentifier { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string Status { get; set; } = "Available";
        public int Version { get; set; } = 0;

        public SECTOR Sector { get; set; } = null!;

        // public RESERVATION? Reservation { get; set; }

    }
}
