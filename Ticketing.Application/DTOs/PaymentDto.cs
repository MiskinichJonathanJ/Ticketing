using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Application.DTOs
{
    public class PaymentDto
    {
        public Guid ReservationId { get; set; }
        public Guid SeatId { get; set; }
        public string SeatStatus { get; set; } = string.Empty;
        public string ReservationStatus { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
