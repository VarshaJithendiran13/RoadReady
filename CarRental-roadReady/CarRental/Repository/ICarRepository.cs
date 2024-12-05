using CarRental.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int carId);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int carId);
    }
}
