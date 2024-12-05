using CarRental.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models.DTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CarId is required.")]
        public int CarId { get; set; }

        // Rating should be between 1 and 5
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        // Comment should have a maximum length
        [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
        public string? Comment { get; set; }

        ///[Required(ErrorMessage = "ReviewDate is required.")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }

        
       
    }
}
