using AutoMapper;
using CarRental.DTOs; // Assuming the DTO is in this namespace
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Models.DTOs;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all actions by default
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentController(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    // GET: api/Payments
    [HttpGet]
    [Authorize(Roles = "User, Admin")] // Allow both User and Admin to view payments
    public async Task<IActionResult> GetAllPayments()
    {
        try
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            var paymentDTOs = _mapper.Map<IEnumerable<PaymentDTO>>(payments); // Mapping to DTOs
            return Ok(paymentDTOs);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // GET: api/Payments/5
    [HttpGet("{paymentId}")]
    [Authorize(Roles = "User, Admin")] // Allow both User and Admin to view a specific payment
    public async Task<IActionResult> GetPaymentById(int paymentId)
    {
        try
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
            {
                throw new NotFoundException("Payment not found");
            }
            var paymentDTO = _mapper.Map<PaymentDTO>(payment); // Mapping to DTO
            return Ok(paymentDTO);
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

    // POST: api/Payments
    [HttpPost]
    // Restrict payment creation to Admin role
    public async Task<IActionResult> AddPayment([FromBody] CreatePaymentDTO createPaymentDTO)
    {
        try
        {
            // Map CreatePaymentDTO to the model (Payment)
            var payment = _mapper.Map<Payment>(createPaymentDTO);
            await _paymentRepository.AddPaymentAsync(payment);
            var createdPaymentDTO = _mapper.Map<PaymentDTO>(payment); // Return the created payment as DTO
            return CreatedAtAction(nameof(GetPaymentById), new { paymentId = payment.PaymentId }, createdPaymentDTO);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("reservation/{reservationId}")]
    [Authorize(Roles = "Host, Admin")] // Allow both User and Admin to view payments by car
    public async Task<IActionResult> GetPaymentsByReservationId(int reservationId)
    {
        try
        {
            var payments = await _paymentRepository.GetPaymentsByReservationIdAsync(reservationId);
            var paymentDTOs = _mapper.Map<IEnumerable<PaymentDTO>>(payments); // Map to DTO
            return Ok(paymentDTOs);
        }
        catch (NotFoundException ex)
        {
            return NotFound(value: ex.Message);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    //// PUT: api/Payments/5
    //[HttpPut("{paymentId}")]
    //[Authorize(Roles = "Admin")] // Restrict payment updates to Admin role
    //public async Task<IActionResult> UpdatePayment(int paymentId, [FromBody] PaymentDTO paymentDTO)
    //{
    //    try
    //    {
    //        if (paymentId != paymentDTO.PaymentId)
    //        {
    //            return BadRequest("Payment ID mismatch.");
    //        }

    //        var payment = _mapper.Map<Payment>(paymentDTO);
    //        await _paymentRepository.UpdatePaymentAsync(payment);
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

    // DELETE: api/Payments/5
    //    [HttpDelete("{paymentId}")]
    //    [Authorize(Roles = "Admin")] // Restrict payment deletion to Admin role
    //    public async Task<IActionResult> DeletePayment(int paymentId)
    //    {
    //        try
    //        {
    //            await _paymentRepository.DeletePaymentAsync(paymentId);
    //            return NoContent();
    //        }
    //        catch (NotFoundException ex)
    //        {
    //            return NotFound(ex.Message);
    //        }
    //        catch (InternalServerException ex)
    //        {
    //            return StatusCode(500, ex.Message);
    //        }
    //    }
}
