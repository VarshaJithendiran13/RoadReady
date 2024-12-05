using CarRental.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        // For login or authentication purposes
        Task<User> GetUserByEmailAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task UpdateUserPasswordAsync(int userId, string newPassword);
        // For login or authentication purposes
    }
}
