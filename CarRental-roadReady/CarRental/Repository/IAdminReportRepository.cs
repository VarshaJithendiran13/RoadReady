using CarRental.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface IAdminReportRepository
    {
        Task<IEnumerable<AdminReport>> GetAllReportsAsync();
        Task<AdminReport> GetReportByIdAsync(int reportId);
        Task AddReportAsync(AdminReport report);
        Task UpdateReportAsync(AdminReport report);
        Task DeleteReportAsync(int reportId);
    }
}
