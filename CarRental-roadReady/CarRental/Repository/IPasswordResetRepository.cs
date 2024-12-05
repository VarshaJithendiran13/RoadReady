using CarRental.Models;
using CarRental.Models.DTOs;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface IPasswordResetRepository
    {
        Task<PasswordReset?> GetResetRequestByTokenAsync(string token);
        Task AddPasswordResetAsync(PasswordReset passwordReset);
        Task MarkTokenAsUsedAsync(string token);
        Task<User?> GetUserByEmailAsync(string email);
        Task UpdateUserPasswordAsync(int userId, string newPassword);
        Task CreatePasswordResetAsync(PasswordResetRequestDTO requestDto);
        Task<PasswordReset?> GetPasswordResetByTokenAsync(string resetToken);
        Task MarkResetTokenAsUsedAsync(int resetId);

    }

}
