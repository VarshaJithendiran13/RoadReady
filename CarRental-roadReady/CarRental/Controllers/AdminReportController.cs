using AutoMapper; // Ensure AutoMapper is installed and configured
using CarRental.Exceptions;
using CarRental.Models;
using CarRental.Repository;
using CarRental.Models.DTOs; // Adjust the namespace as needed
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarRental.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminReportController : ControllerBase
{
    private readonly IAdminReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public AdminReportController(IAdminReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    [HttpGet("{reportId}")]
    public async Task<IActionResult> GetReportById(int reportId)
    {
        try
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            var reportDto = _mapper.Map<AdminReportReadDTO>(report);
            return Ok(reportDto);
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

    [HttpPost]
    public async Task<IActionResult> AddReport([FromBody] AdminReportCreateDTO reportDto)
    {
        try
        {
            var report = _mapper.Map<AdminReport>(reportDto);
            await _reportRepository.AddReportAsync(report);
            var createdReportDto = _mapper.Map<AdminReportReadDTO>(report);
            return CreatedAtAction(nameof(GetReportById), new { reportId = report.ReportId }, createdReportDto);
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

    [HttpPut("{reportId}")]
    public async Task<IActionResult> UpdateReport(int reportId, [FromBody] AdminReportCreateDTO reportDto)
    {
        try
        {
            var report = _mapper.Map<AdminReport>(reportDto);
            report.ReportId = reportId; // Ensure ID is set for update
            await _reportRepository.UpdateReportAsync(report);
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

    [HttpDelete("{reportId}")]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try
        {
            await _reportRepository.DeleteReportAsync(reportId);
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
