using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly YourDbContext _context;

        public CarRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all cars
        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            try
            {
                return await _context.Cars.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the cars: {ex.Message}");
            }
        }

        // Get a car by ID
        public async Task<Car> GetCarByIdAsync(int carId)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.CarId == carId);

            if (car == null)
                throw new NotFoundException($"Car with ID {carId} not found.");

            return car;
        }

        // Add a new car
        public async Task AddCarAsync(Car car)
        {
            if (car == null)
                throw new ValidationException("Car details cannot be null.");

            var existingCar = await _context.Cars
                .FirstOrDefaultAsync(c => c.CarId == car.CarId);

            if (existingCar != null)
                throw new DuplicateResourceException($"Car with ID {car.CarId} already exists.");

            try
            {
                await _context.Cars.AddAsync(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch database-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the car: {ex.Message}");
            }
        }

        // Update an existing car
        public async Task UpdateCarAsync(Car updatedCar)
        {
            if (updatedCar == null)
                throw new ValidationException("Updated car details cannot be null.");

            var existingCar = await _context.Cars.FindAsync(updatedCar.CarId);

            if (existingCar == null)
                throw new NotFoundException($"Car with ID {updatedCar.CarId} not found.");

            // Update car details
            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.Specifications = updatedCar.Specifications;
            existingCar.PricePerDay = updatedCar.PricePerDay;
            existingCar.AvailabilityStatus = updatedCar.AvailabilityStatus;
            existingCar.Location = updatedCar.Location;
            existingCar.ImageUrl = updatedCar.ImageUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the car: {ex.Message}");
            }
        }

        // Delete a car by ID
        public async Task DeleteCarAsync(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);

            if (car == null)
                throw new NotFoundException($"Car with ID {carId} not found.");

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
    }
}
