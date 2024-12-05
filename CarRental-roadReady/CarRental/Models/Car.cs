using System;
using System.Collections.Generic;

namespace CarRental.Models;

public partial class Car
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

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
