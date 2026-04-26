namespace Ticketing.Application.DTOs
{
    public class ReservationDto
    {
        public Guid ReservationId { get; set; }
        public Guid SeatId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string? Message { get; set; } = string.Empty;
    }
}
