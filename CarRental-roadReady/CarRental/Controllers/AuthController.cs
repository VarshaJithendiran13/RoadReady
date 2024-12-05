using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.DTOs;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;

        public AuthController(
            IMapper mapper,
            IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepository,
            JwtTokenService jwtTokenService,
            IEmailService emailService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordResetRepository = passwordResetRepository;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
        }


        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userRegistrationDTO);
                await _userRepository.AddUserAsync(user);
                var createdUserDTO = _mapper.Map<UserDTO>(user);
                return CreatedAtAction(nameof(Register), new { userId = user.UserId }, createdUserDTO);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateResourceException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InternalServerException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            try
            {
                var user = await _userRepository.ValidateUserAsync(request.Email, request.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                var token = _jwtTokenService.GenerateToken(user.UserId, user.Role);
                var response = new UserLoginResponseDTO { Token = token };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: api/Auth/forgot-password
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetRequestDTO request)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound("User not found.");
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

                await _passwordResetRepository.AddPasswordResetAsync(passwordReset);

                // Send the reset token via email
                await _emailService.SendEmailAsync(request.Email, "Password Reset",
                    $"Your password reset token is: {resetToken}");

                return Ok("Password reset email sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // POST: api/Auth/reset-password
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetConfirmDTO request)
        {
            try
            {
                var resetEntry = await _passwordResetRepository.GetPasswordResetByTokenAsync(request.ResetToken);
                if (resetEntry == null || resetEntry.IsUsed || resetEntry.ExpirationDate < DateTime.UtcNow)
                {
                    return BadRequest("Invalid or expired token.");
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return BadRequest("Passwords do not match.");
                }

                await _userRepository.UpdateUserPasswordAsync(resetEntry.UserId, request.NewPassword);
                await _passwordResetRepository.MarkResetTokenAsUsedAsync(resetEntry.ResetId);

                return Ok("Password successfully reset.");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
