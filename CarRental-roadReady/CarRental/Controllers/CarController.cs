using AutoMapper;
using CarRental.DTOs;
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication for all actions in this controller
public class CarController : ControllerBase
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper; // Assuming you're using AutoMapper for mapping

    public CarController(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    [HttpGet]
    //[Authorize(Roles = "Admin,User")]
    [AllowAnonymous] // Make this endpoint publicly accessible if needed
    public async Task<IActionResult> GetAllCars()
    {
        try
        {
            var cars = await _carRepository.GetAllCarsAsync();
            var carDtos = _mapper.Map<IEnumerable<CarReadDTO>>(cars);
            return Ok(carDtos);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{carId}")]
    //[Authorize(Roles = "Admin,User,Host")]
    public async Task<IActionResult> GetCarById(int carId)
    {
        try
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }
            var carDto = _mapper.Map<CarReadDTO>(car);
            return Ok(carDto);
        }
        catch (InternalServerException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Host,User")] // Require Admin role for adding a car
    public async Task<IActionResult> AddCar([FromBody] CarCreateDTO carDto)
    {
        try
        {
            var car = _mapper.Map<Car>(carDto);
            await _carRepository.AddCarAsync(car);
            var carReadDto = _mapper.Map<CarReadDTO>(car);
            return CreatedAtAction(nameof(GetCarById), new { carId = car.CarId }, carReadDto);
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

    [HttpPut]
    [Authorize(Roles = "Admin,Host")] // Require Admin role for updating a car
    public async Task<IActionResult> UpdateCar([FromBody] CarUpdateDTO carDto)
    {
        try
        {
            var car = _mapper.Map<Car>(carDto);
            await _carRepository.UpdateCarAsync(car);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpDelete("{carId}")]
    [Authorize(Roles = "Admin,Host")] // Require Admin role for deleting a car
    public async Task<IActionResult> DeleteCar(int carId)
    {
        try
        {
            await _carRepository.DeleteCarAsync(carId);
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
}
