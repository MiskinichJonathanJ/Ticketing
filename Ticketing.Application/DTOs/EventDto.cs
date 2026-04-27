namespace Ticketing.Application.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = null!;
        public string Status { get; set; } = null!;
        
    }
}
