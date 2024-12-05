using System;
using System.Collections.Generic;

namespace CarRental.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public  DateTime PickupDate { get; set; }

    public DateTime DropoffDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string? ReservationStatus { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
