using CarRental.Validations;

using System.ComponentModel.DataAnnotations;

namespace CarRental.DTOs
{
    public class CreateReservationDTO
    {
       

        //[Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        //  [Required(ErrorMessage = "CarId is required.")]
        public int CarId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DropOffDate { get; set; }

        [Required(ErrorMessage = "TotalPrice is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "TotalPrice must be greater than 0.")]
        public decimal TotalPrice { get; set; }

        [StringLength(50, ErrorMessage = "ReservationStatus cannot exceed 50 characters.")]
        public string? ReservationStatus { get; set; }
    }
}
