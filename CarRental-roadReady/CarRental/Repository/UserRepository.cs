using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly YourDbContext _context;

        public UserRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the users: {ex.Message}");
            }
        }

        public async Task UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            // Add password hashing here if required
            user.Password = newPassword;

            await _context.SaveChangesAsync();
        }


        // Get a user by ID
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            return user;
        }

        // Add a new user
        public async Task AddUserAsync(User user)
        {
            if (user == null)
                throw new ValidationException("User details cannot be null.");

            // Check if user already exists by email (assuming email is unique)
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
                throw new DuplicateResourceException($"A user with email {user.Email} already exists.");

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch database-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the user: {ex.Message}");
            }
        }

        // Update an existing user
        public async Task UpdateUserAsync(User updatedUser)
        {
            if (updatedUser == null)
                throw new ValidationException("Updated user details cannot be null.");

            var existingUser = await _context.Users.FindAsync(updatedUser.UserId);

            if (existingUser == null)
                throw new NotFoundException($"User with ID {updatedUser.UserId} not found.");

            // Update user details
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the user: {ex.Message}");
            }
        }

        // Delete a user by ID
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                throw new NotFoundException($"User with ID {userId} not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // Get user by email (for login/authentication purposes)
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new NotFoundException($"User with email {email} not found.");

            return user;
        }
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Check if the user exists and if the password matches
            if (user == null || user.Password != password)
            {
                return null; // Return null if credentials are invalid
            }

            // Return the user if valid
            return user;
        }
    }
}
