using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRestoranApi.Data;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReservationController> _logger;
    private const int DefaultEmployeeId = 1; // ������ ��������� ID ���������� = 1

    public ReservationController(AppDbContext context, ILogger<ReservationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // �������� ������������
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        _logger.LogInformation($"������ �� ������������: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // ���������, ���������� �� ����
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                _logger.LogWarning("���� �� ������.");
                return NotFound("���� �� ������.");
            }

            // ���������, ��� �� ����������� ������������ �� ��� �����
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
                _logger.LogWarning("���� ���� ��� ������������ �� ��� �����.");
                return BadRequest("���� ���� ��� ������������ �� ��� �����.");
            }

            // ������� ������������
            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = request.ReservationTime,
                EmployeeId = DefaultEmployeeId, // ������ ID = 1
                CreatedAt = DateTime.UtcNow,
                Status = Reservation.ReservationStatus.Reserved // ������������� ������ "Reserved"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"������������ ������� ������� ��� ������� {request.CustomerId}, ���� {request.TableId}.");

            return Ok(new
            {
                message = "���� ������� ������������!",
                reservationId = reservation.Id,
                employeeId = DefaultEmployeeId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"������ �������: {ex.Message}");
            return StatusCode(500, $"������ �������: {ex.Message}");
        }
    }

    // �������� ������ ���� ������������
    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        try
        {
            var reservations = await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Table)
                .ToListAsync();

            return Ok(reservations);
        }
        catch (Exception ex)
        {
            _logger.LogError($"������ ��� ��������� ������������: {ex.Message}");
            return StatusCode(500, $"������ �������: {ex.Message}");
        }
    }

    // �������� ������ ���� ������
    [HttpGet("tables")]
    public async Task<IActionResult> GetTables()
    {
        try
        {
            var tables = await _context.Tables.ToListAsync();
            return Ok(tables);
        }
        catch (Exception ex)
        {
            _logger.LogError($"������ ��� ��������� ������: {ex.Message}");
            return StatusCode(500, $"������ �������: {ex.Message}");
        }
    }
}
