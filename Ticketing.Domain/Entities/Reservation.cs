using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; }
        public DateTime ExpireAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
    }
}
