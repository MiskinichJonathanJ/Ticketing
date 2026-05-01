using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid SeatId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ExpireAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
    }
}
