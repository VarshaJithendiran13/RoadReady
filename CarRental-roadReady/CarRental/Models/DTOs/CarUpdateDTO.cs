using System.ComponentModel.DataAnnotations;

namespace CarRental.DTOs
{
    public class CarUpdateDTO
    {
        [Required(ErrorMessage = "Car ID is required.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Make is required.")]
        public string Make { get; set; } = null!;

        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; } = null!;

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        public string? Specifications { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price per day must be a positive value.")]
        public decimal PricePerDay { get; set; }

        public bool AvailabilityStatus { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = null!;

        [Url(ErrorMessage = "Invalid image URL format.")]
        public string? ImageUrl { get; set; }
    }
}
