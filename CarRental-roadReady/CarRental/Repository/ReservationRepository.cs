using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly YourDbContext _context;

        public ReservationRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all reservations
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            try
            {
                return await _context.Reservations.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the reservations: {ex.Message}");
            }
        }

        // Get a reservation by ID
        public async Task<Reservation> GetReservationByIdAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

            if (reservation == null)
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");

            return reservation;
        }

        // Add a new reservation
        public async Task AddReservationAsync(Reservation reservation)
        {
            if (reservation == null)
                throw new ValidationException("Reservation details cannot be null.");

            // Check for duplicate reservation if needed
            var existingReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.UserId == reservation.UserId && r.CarId == reservation.CarId
                                          && r.PickupDate == reservation.PickupDate);
            if (existingReservation != null)
                throw new DuplicateResourceException("A reservation already exists with the same user, car, and pickup date.");

            try
            {
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch database-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the reservation: {ex.Message}");
            }
        }

        // Update an existing reservation
        public async Task UpdateReservationAsync(Reservation updatedReservation)
        {
            if (updatedReservation == null)
                throw new ValidationException("Updated reservation details cannot be null.");

            var existingReservation = await _context.Reservations.FindAsync(updatedReservation.ReservationId);

            if (existingReservation == null)
                throw new NotFoundException($"Reservation with ID {updatedReservation.ReservationId} not found.");

            // Update reservation details
            //existingReservation.UserId = updatedReservation.UserId;
            existingReservation.CarId = updatedReservation.CarId;
            existingReservation.PickupDate = updatedReservation.PickupDate;
            existingReservation.DropoffDate = updatedReservation.DropoffDate;
            existingReservation.TotalPrice = updatedReservation.TotalPrice;
            existingReservation.ReservationStatus = updatedReservation.ReservationStatus;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the reservation: {ex.Message}");
            }
        }

        // Delete a reservation by ID
        public async Task DeleteReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);

            if (reservation == null)
                throw new NotFoundException($"Reservation with ID {reservationId} not found.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        // Get reservations by user ID
        public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.UserId == userId)
                    .ToListAsync();

                if (reservations == null || reservations.Count == 0)
                    throw new NotFoundException($"No reservations found for user ID {userId}.");

                return reservations;
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the reservations for user {userId}: {ex.Message}");
            }
        }

        // Get reservations by car ID
        public async Task<IEnumerable<Reservation>> GetReservationsByCarIdAsync(int carId)
        {
            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.CarId == carId)
                    .ToListAsync();

                if (reservations == null || reservations.Count == 0)
                    throw new NotFoundException($"No reservations found for car ID {carId}.");

                return reservations;
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the reservations for car {carId}: {ex.Message}");
            }
        }
    }
}
