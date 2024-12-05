using AutoMapper;

using CarRental;
using CarRental.DTOs;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.DTOs;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;  // For PasswordHasher
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Enforce authentication for all actions in this controller
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly JwtTokenService _jwtTokenService;

    public UserController(IUserRepository userRepository, IMapper mapper, JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users); // Map to DTOs
            return Ok(userDTOs);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/Users/{userId}
  
    


    // POST: api/Users (User Registration)
    [HttpPost]
    [Authorize(Roles = "Admin")] // Only Admins can add users
    public async Task<IActionResult> AddUser([FromBody] UserRegistrationDTO userRegistrationDTO)
    {
        try
        {
            var user = _mapper.Map<User>(userRegistrationDTO); // Map DTO to User model
            await _userRepository.AddUserAsync(user);
            var createdUserDTO = _mapper.Map<UserDTO>(user); // Map to DTO for response
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, createdUserDTO);
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

    
   

    // DELETE: api/Users/{userId}
    [HttpDelete]
    [Authorize(Roles = "User, Admin")] // Both Users and Admins can delete
    public async Task<IActionResult> DeleteUser()
    {
        try
        {
            // Extract userId from JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Delete the user
            await _userRepository.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    // GET: api/Users/profile
    [HttpGet("profile")]
    [Authorize(Roles = "User, Admin")] // Both Users and Admins can access
    public async Task<IActionResult> GetUserById()
    {
        try
        {
            // Extract userId from the JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Fetch user details for the logged-in user
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            // Map to DTO
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    // PUT: api/Users (User Update)
    [HttpPut]
    [Authorize(Roles = "Admin, User")] // Admins and the user themselves can update user details
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
    {
        try
        {
            // Extract userId from the JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int loggedInUserId = int.Parse(userIdClaim.Value);

            // Fetch the existing user to verify the existence
            var existingUser = await _userRepository.GetUserByIdAsync(loggedInUserId);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            // Allow only the logged-in user or Admin to update
            if (User.IsInRole("User") && existingUser.UserId != loggedInUserId)
            {
                return Forbid("You are not authorized to update another user's details.");
            }

            // Map updated details from the UserUpdateDTO to the User model
            var user = _mapper.Map<User>(userUpdateDTO);

            // Optionally, set the UserId from the logged-in user if the user update DTO doesn't include it
            user.UserId = loggedInUserId;

            // Save changes to the user
            await _userRepository.UpdateUserAsync(user);

            return NoContent(); // Indicate successful update with no content
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message); // Return Not Found if user doesn't exist
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message); // Return Bad Request if validation fails
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message); // Return internal server error for unexpected issues
        }
    }





}
