using CarRental.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int reservationId);
        Task AddReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int reservationId);
        Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);
        Task<IEnumerable<Reservation>> GetReservationsByCarIdAsync(int carId);
    }
}
