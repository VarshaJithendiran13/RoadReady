using System;
using System.Collections.Generic;

namespace CarRental.Models;

public partial class PasswordReset
{
    public int ResetId { get; set; }

    public int UserId { get; set; }

    public string ResetToken { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
