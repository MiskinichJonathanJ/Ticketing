namespace Ticketing.Application.DTOs
{
    public class SeatDto
    {
        public Guid Id { get; set; }
        public string RowIdentifier { get; set; } = null!;
        public int SeatNumber { get; set; }
        public string Status { get; set; } = null!;
    }
}
