using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class AdminReportRepository : IAdminReportRepository
    {
        private readonly YourDbContext _context;

        public AdminReportRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all reports
        public async Task<IEnumerable<AdminReport>> GetAllReportsAsync()
        {
            try
            {
                return await _context.AdminReports.ToListAsync();
            }
            catch (Exception ex)
            {
                // This could be logged or rethrown as an internal error
                throw new InternalServerException($"An error occurred while retrieving the reports: {ex.Message}");
            }
        }

        // Get a report by ID
        public async Task<AdminReport> GetReportByIdAsync(int reportId)
        {
            var report = await _context.AdminReports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);

            if (report == null)
                throw new NotFoundException($"Report with ID {reportId} not found.");

            return report;
        }

        // Add a new report
        public async Task AddReportAsync(AdminReport report)
        {
            if (report == null)
                throw new ValidationException("Report cannot be null.");

            var existingReport = await _context.AdminReports
                .FirstOrDefaultAsync(r => r.ReportId == report.ReportId);

            if (existingReport != null)
                throw new DuplicateResourceException($"Report with ID {report.ReportId} already exists.");

            try
            {
                await _context.AdminReports.AddAsync(report);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch DB-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the report: {ex.Message}");
            }
        }

        // Update an existing report
        public async Task UpdateReportAsync(AdminReport updatedReport)
        {
            if (updatedReport == null)
                throw new ValidationException("Updated report details cannot be null.");

            var existingReport = await _context.AdminReports.FindAsync(updatedReport.ReportId);

            if (existingReport == null)
                throw new NotFoundException($"Report with ID {updatedReport.ReportId} not found.");

            // Update report details
            existingReport.ReportDate = updatedReport.ReportDate;
            existingReport.TotalReservations = updatedReport.TotalReservations;
            existingReport.TotalRevenue = updatedReport.TotalRevenue;
            existingReport.TopCars = updatedReport.TopCars;
            existingReport.MostActiveUser = updatedReport.MostActiveUser;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the report: {ex.Message}");
            }
        }

        // Delete a report by ID
        public async Task DeleteReportAsync(int reportId)
        {
            var report = await _context.AdminReports.FindAsync(reportId);

            if (report == null)
                throw new NotFoundException($"Report with ID {reportId} not found.");

            _context.AdminReports.Remove(report);
            await _context.SaveChangesAsync();
        }
    }
}
