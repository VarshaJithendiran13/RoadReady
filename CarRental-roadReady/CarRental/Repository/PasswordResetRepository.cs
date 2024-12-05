using CarRental.Models;
using CarRental.Models.DTOs;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly YourDbContext _context;

        public PasswordResetRepository(YourDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordReset?> GetResetRequestByTokenAsync(string token)
        {
            return await _context.PasswordResets
                .FirstOrDefaultAsync(pr => pr.ResetToken == token && pr.ExpirationDate > DateTime.Now && !pr.IsUsed);
        }

        public async Task AddPasswordResetAsync(PasswordReset passwordReset)
        {
            await _context.PasswordResets.AddAsync(passwordReset);
            await _context.SaveChangesAsync();
        }

        public async Task MarkTokenAsUsedAsync(string token)
        {
            var resetRequest = await _context.PasswordResets.FirstOrDefaultAsync(pr => pr.ResetToken == token);
            if (resetRequest != null)
            {
                resetRequest.IsUsed = true;
                _context.PasswordResets.Update(resetRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Password = newPassword; // Ensure proper hashing
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task CreatePasswordResetAsync(PasswordResetRequestDTO requestDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == requestDto.Email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var resetToken = Guid.NewGuid().ToString();
            var expirationDate = DateTime.UtcNow.AddHours(8);

            var passwordReset = new PasswordReset
            {
                UserId = user.UserId,
                ResetToken = resetToken,
                ExpirationDate = expirationDate,
                IsUsed = false
            };

            await _context.PasswordResets.AddAsync(passwordReset);
            await _context.SaveChangesAsync();

            // Send reset token via email (invoke email service here)
        }

        public async Task<PasswordReset?> GetPasswordResetByTokenAsync(string resetToken)
        {
            return await _context.PasswordResets
                .FirstOrDefaultAsync(pr => pr.ResetToken == resetToken);
        }

        public async Task MarkResetTokenAsUsedAsync(int resetId)
        {
            var resetEntry = await _context.PasswordResets.FindAsync(resetId);
            if (resetEntry == null)
            {
                throw new NotFoundException($"Password reset with ID {resetId} not found.");
            }

            resetEntry.IsUsed = true;
            await _context.SaveChangesAsync();
        }


    }

}
