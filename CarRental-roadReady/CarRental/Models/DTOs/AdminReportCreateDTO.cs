using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.DTOs
{
    public class AdminReportCreateDTO
    {
        [Required(ErrorMessage = "Report date is required.")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Total reservations must be a positive value.")]
        public int? TotalReservations { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total revenue must be a positive value.")]
        public decimal? TotalRevenue { get; set; }

        public string? TopCars { get; set; }
        public string? MostActiveUser { get; set; }
    }
}
