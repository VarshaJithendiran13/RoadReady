using System;
using System.Collections.Generic;

namespace CarRental.Models;

public partial class AdminReport
{
    public int ReportId { get; set; }

    public DateTime ReportDate { get; set; }

    public int? TotalReservations { get; set; }

    public decimal? TotalRevenue { get; set; }

    public string? TopCars { get; set; }

    public string? MostActiveUser { get; set; }
}
