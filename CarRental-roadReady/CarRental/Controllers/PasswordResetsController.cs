//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using CarRental.Models;
//using CarRental.Exceptions;
//using CarRental.Repository;
//using CarRental.Models.DTOs;
//using CarRental.Validations;
//using AutoMapper;

//namespace CarRental.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class PasswordResetsController : ControllerBase
//    {
//        private readonly IPasswordResetRepository _passwordResetRepository;
//        private readonly IMapper _mapper;

//        public PasswordResetsController(IPasswordResetRepository passwordResetRepository, IMapper mapper)
//        {
//            _passwordResetRepository = passwordResetRepository;
//            _mapper = mapper;
//        }

//        // GET: api/PasswordResets
//        [HttpGet]
//        [Authorize(Roles = "Admin")]
//        public async Task<ActionResult<IEnumerable<PasswordResetResponseDTO>>> GetPasswordResets()
//        {
//            try
//            {
//                var passwordResets = await _passwordResetRepository.GetAllPasswordResetsAsync();
//                var passwordResetDtos = _mapper.Map<IEnumerable<PasswordResetResponseDTO>>(passwordResets);
//                return Ok(passwordResetDtos);
//            }
//            catch (Exception)
//            {
//                return StatusCode(500, "An error occurred while retrieving the password resets.");
//            }
//        }

//        // GET: api/PasswordResets/5
//        [HttpGet("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<ActionResult<PasswordResetResponseDTO>> GetPasswordReset(int id)
//        {
//            try
//            {
//                var passwordReset = await _passwordResetRepository.GetPasswordResetByIdAsync(id);
//                if (passwordReset == null)
//                {
//                    throw new NotFoundException($"Password reset with ID {id} not found.");
//                }
//                var passwordResetDto = _mapper.Map<PasswordResetResponseDTO>(passwordReset);
//                return Ok(passwordResetDto);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (Exception)
//            {
//                return StatusCode(500, "An error occurred while retrieving the password reset.");
//            }
//        }

//        // PUT: api/PasswordResets/5
//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> PutPasswordReset(int id, PasswordResetDTO passwordResetDto)
//        {
//            if (id != passwordResetDto.ResetId)
//            {
//                return BadRequest("Password reset ID mismatch.");
//            }

//            try
//            {
//                // Call the repository method directly with PasswordResetDTO
//                await _passwordResetRepository.UpdatePasswordResetAsync(id, passwordResetDto);

//                return NoContent();
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception)
//            {
//                return StatusCode(500, "An error occurred while updating the password reset.");
//            }
//        }

//        // POST: api/PasswordResets
//        [HttpPost]
//        [AllowAnonymous]
//        public async Task<ActionResult<PasswordResetResponseDTO>> PostPasswordReset(PasswordResetRequestDTO passwordResetDto)
//        {
//            try
//            {
//                // Map the PasswordResetRequestDTO to PasswordResetDTO
//                var passwordResetDTO = _mapper.Map<PasswordResetDTO>(passwordResetDto);

//                // Call the repository method with the PasswordResetDTO
//                var createdPasswordReset = await _passwordResetRepository.AddPasswordResetAsync(passwordResetDTO);

//                // Map the created PasswordResetDTO to PasswordResetResponseDTO
//                var createdPasswordResetDto = _mapper.Map<PasswordResetResponseDTO>(createdPasswordReset);

//                // Return the response
//                return CreatedAtAction(nameof(GetPasswordReset), new { id = createdPasswordResetDto.ResetId }, createdPasswordResetDto);
//            }
//            catch (DuplicateResourceException ex)
//            {
//                return Conflict(ex.Message);
//            }
//            catch (ValidationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception)
//            {
//                return StatusCode(500, "An error occurred while creating the password reset.");
//            }
//        }

//        // DELETE: api/PasswordResets/5
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeletePasswordReset(int id)
//        {
//            try
//            {
//                await _passwordResetRepository.DeletePasswordResetAsync(id);
//                return NoContent();
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(ex.Message);
//            }
//            catch (Exception)
//            {
//                return StatusCode(500, "An error occurred while deleting the password reset.");
//            }
//        }
//    }
//}
