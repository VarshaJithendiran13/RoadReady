using AutoMapper;
using CarRental.DTOs; // Assuming the ReviewDTO is in this namespace
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.DTOs;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all actions
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;

    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    // GET: api/Reviews
    [HttpGet]
    [Authorize(Roles = "User, Admin")] // Allow both User and Admin to get all reviews
    public async Task<IActionResult> GetAllReviews()
    {
        try
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews); // Map to DTO
            return Ok(reviewDTOs);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/Reviews/{reviewId}
    [HttpGet("{reviewId}")]
    [Authorize(Roles = "User, Admin")] // Allow both User and Admin to get a specific review
    public async Task<IActionResult> GetReviewById(int reviewId)
    {
        try
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                throw new NotFoundException("Review not found");
            }
            var reviewDTO = _mapper.Map<ReviewDTO>(review); // Map to DTO
            return Ok(reviewDTO);
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

    // POST: api/Reviews
    [HttpPost]
    [Authorize(Roles = "User")] // Only Users can add reviews
    public async Task<IActionResult> AddReview([FromBody] CreateReviewDTO reviewDTO)
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

            // Set the extracted userId into the reviewDTO
            reviewDTO.UserId = userId;

            // Map from DTO to model
            var review = _mapper.Map<Review>(reviewDTO);

            // Add review to the database
            await _reviewRepository.AddReviewAsync(review);

            // Map to DTO for response
            var createdReviewDTO = _mapper.Map<CreateReviewDTO>(review);

            return CreatedAtAction(nameof(GetReviewById), new { reviewId = review.ReviewId }, createdReviewDTO);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DuplicateResourceException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // Get reviews by Car ID
    [HttpGet("car/{carId}")]
    [Authorize(Roles = "Host, Admin, User")] // Allow both User and Admin to view reviews by car
    public async Task<IActionResult> GetReviewByCarId(int carId)
    {
        try
        {
            var review = await _reviewRepository.GetReviewsByCarIdAsync(carId);
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(review); // Map to DTO
            return Ok(reviewDTOs);
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
}

        //// PUT: api/Reviews
        //[HttpPut]
        //[Authorize(Roles = "User, Admin")] // Allow Users to update their own reviews and Admin to update any review
        //public async Task<IActionResult> UpdateReview([FromBody] ReviewDTO reviewDTO)
        //{
        //    try
        //    {
        //        var review = _mapper.Map<Review>(reviewDTO); // Map from DTO to model
        //        await _reviewRepository.UpdateReviewAsync(review);
        //        return NoContent();
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (InternalServerException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        // DELETE: api/Reviews/{reviewId}
        //[HttpDelete("{reviewId}")]
        //[Authorize(Roles = "User, Admin")] // Allow Users to delete their own reviews and Admin to delete any review
        //public async Task<IActionResult> DeleteReview(int reviewId)
        //{
        //    try
        //    {
        //        await _reviewRepository.DeleteReviewAsync(reviewId);
        //        return NoContent();
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InternalServerException ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    
// GET: api/Reviews/{reviewId}
//[HttpGet("{carId}")]
//[Authorize(Roles = "User, Admin")] // Allow both User and Admin to get a specific review
//public async Task<IActionResult> GetReviewById(int carId)
//{
//    try
//    {
//        var review = await _reviewRepository.GetReviewByIdAsync();
//        if (review == null)
//        {
//            throw new NotFoundException("car not found");
//        }
//        var reviewDTO = _mapper.Map<ReviewDTO>(review); // Map to DTO
//        return Ok(reviewDTO);
//    }
//    catch (NotFoundException ex)
//    {
//        return NotFound(ex.Message);
//    }
//    catch (InternalServerException ex)
//    {
//        return StatusCode(500, ex.Message);
//    }
 
   
