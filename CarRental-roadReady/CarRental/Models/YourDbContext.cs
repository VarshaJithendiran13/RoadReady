using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Models;

public partial class YourDbContext : DbContext
{
    public YourDbContext()
    {
    }

    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminReport> AdminReports { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-J7MQ370B;Database=CarRental;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__AdminRep__D5BD48E583C2CF4F");

            entity.ToTable("AdminReport");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.MostActiveUser).HasMaxLength(255);
            entity.Property(e => e.TopCars).HasMaxLength(255);
            entity.Property(e => e.TotalRevenue).HasColumnType("decimal(15, 2)");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Cars__68A0340E29449554");

            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.AvailabilityStatus).HasDefaultValue(true);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Make).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.PricePerDay).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.HasKey(e => e.ResetId).HasName("PK__Password__783CF04DE2F1672B");

            entity.ToTable("PasswordReset");

            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.ResetToken).HasMaxLength(100);

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordR__UserI__5CD6CB2B");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58EB611713");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.PaymentMethod).HasMaxLength(20);
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.Status).HasMaxLength(10);

            entity.HasOne(d => d.Reservation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Reserv__44FF419A");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F04AD0DD120");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.ReservationStatus).HasMaxLength(15);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Car).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__CarID__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__UserI__3F466844");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AED7FA1129");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CarId).HasColumnName("CarID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Car).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__CarID__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__UserID__49C3F6B7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD680F3FC");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105340C05488A").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
