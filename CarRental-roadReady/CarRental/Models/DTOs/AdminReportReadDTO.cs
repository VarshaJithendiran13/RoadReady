namespace CarRental.DTOs
{
    public class AdminReportReadDTO
    {
        public int ReportId { get; set; }
        public DateTime ReportDate { get; set; }
        public int? TotalReservations { get; set; }
        public decimal? TotalRevenue { get; set; }
        public string? TopCars { get; set; }
        public string? MostActiveUser { get; set; }
    }
}
