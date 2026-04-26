namespace Ticketing.Application.DTOs
{
    public class SectorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
    }
}
