using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly YourDbContext _context;

        public PaymentRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all payments
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            try
            {
                return await _context.Payments.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the payments: {ex.Message}");
            }
        }

        // Get a payment by ID
        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null)
                throw new NotFoundException($"Payment with ID {paymentId} not found.");

            return payment;
        }

        // Add a new payment
        public async Task AddPaymentAsync(Payment payment)
        {
            if (payment == null)
                throw new ValidationException("Payment details cannot be null.");

            // You can add any additional validation for duplicate payments or specific business logic here

            try
            {
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch database-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the payment: {ex.Message}");
            }
        }

        // Update an existing payment
        public async Task UpdatePaymentAsync(Payment updatedPayment)
        {
            if (updatedPayment == null)
                throw new ValidationException("Updated payment details cannot be null.");

            var existingPayment = await _context.Payments.FindAsync(updatedPayment.PaymentId);

            if (existingPayment == null)
                throw new NotFoundException($"Payment with ID {updatedPayment.PaymentId} not found.");

            // Update payment details
            existingPayment.Amount = updatedPayment.Amount;
            existingPayment.PaymentDate = updatedPayment.PaymentDate;
            existingPayment.PaymentMethod = updatedPayment.PaymentMethod;
            existingPayment.Status = updatedPayment.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the payment: {ex.Message}");
            }
        }

        // Delete a payment by ID
        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);

            if (payment == null)
                throw new NotFoundException($"Payment with ID {paymentId} not found.");

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Payment>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            try
            {
                var payments = await _context.Payments
                    .Where(r => r.ReservationId == reservationId)
                    .ToListAsync();

                if (payments == null || payments.Count == 0)
                    throw new NotFoundException($"No reservations found for car ID {reservationId}.");

                return payments;
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the payments for car {reservationId}: {ex.Message}");
            }
        }
    }
}
