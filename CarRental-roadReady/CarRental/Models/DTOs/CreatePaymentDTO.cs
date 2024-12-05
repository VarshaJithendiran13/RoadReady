using CarRental.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.DTOs
{
    public class CreatePaymentDTO
    {
        [Required(ErrorMessage = "ReservationId is required.")]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [StringLength(50, ErrorMessage = "PaymentMethod cannot exceed 50 characters.")]
        public string? PaymentMethod { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [RegularExpression(@"^(Pending|Completed|Failed|Refunded)$", ErrorMessage = "Status must be one of the following: Pending, Completed, Failed, Refunded.")]
        public string? Status { get; set; }
    }
}
