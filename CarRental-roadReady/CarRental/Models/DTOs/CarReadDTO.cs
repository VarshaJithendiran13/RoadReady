namespace CarRental.DTOs
{
    public class CarReadDTO
    {
        public int CarId { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string? Specifications { get; set; }
        public decimal PricePerDay { get; set; }
        public bool AvailabilityStatus { get; set; }
        public string Location { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
