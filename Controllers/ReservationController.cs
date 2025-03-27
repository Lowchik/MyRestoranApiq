using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private const int DefaultEmployeeId = 1; // ������ ��������� ID ���������� = 1

    public ReservationController(AppDbContext context)
    {
        _context = context;
    }

    // ? �������� ������������
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        Console.WriteLine($"������ �� ������������: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // ���������, ���������� �� ����
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                return NotFound("���� �� ������.");
            }

            // ���������, ��� �� ����������� ������������ �� ��� �����
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
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

            return Ok(new
            {
                message = "���� ������� ������������!",
                reservationId = reservation.Id,
                employeeId = DefaultEmployeeId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"������ �������: {ex.Message}");
        }
    }

    // ? �������� ������ ���� ������������
    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .ToListAsync();

        return Ok(reservations);
    }

    // ? �������� ������������ �� ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById(int id)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation == null)
        {
            return NotFound("������������ �� �������.");
        }

        return Ok(reservation);
    }

    // ? �������� ������ ���� ������
    [HttpGet("tables")]
    public async Task<IActionResult> GetTables()
    {
        var tables = await _context.Tables.ToListAsync();
        return Ok(tables);
    }

    // ? �������� ���� �� ID
    [HttpGet("tables/{id}")]
    public async Task<IActionResult> GetTableById(int id)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table == null)
        {
            return NotFound("���� �� ������.");
        }

        return Ok(table);
    }
}
