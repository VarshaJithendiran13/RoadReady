using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRental
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int userId, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Ensure the key is at least 256 bits (32 bytes) long
            var keyString = jwtSettings["Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new ArgumentNullException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims
            var claims = new[]
            {
        new Claim(ClaimTypes.Role, role), // Role claim
        new Claim("userId", userId.ToString()), // User ID claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
    };

            // Generate token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Use UTC for consistency
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to ensure the key is at least 256 bits
        private SymmetricSecurityKey EnsureKeySize(string key)
        {
            // Convert the key to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Ensure the key size is at least 256 bits (32 bytes)
            if (keyBytes.Length < 32)
            {
                throw new ArgumentException("The key must be at least 256 bits (32 bytes) in length.");
            }

            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
