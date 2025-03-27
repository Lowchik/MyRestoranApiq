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
    public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
    {
        Console.WriteLine($"������ �� ������������: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // 1?? ���������, ���������� �� ����
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                return NotFound("���� �� ������.");
            }

            // 2?? ���������, �������� �� ����
            if (table.Status != "available")
            {
                return BadRequest("���� ���� ��� ������������.");
            }

            // 3?? ���������, ��� �� ����������� ������������ �� ��� �����
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
                return BadRequest("���� ���� ��� ������������ �� ��� �����.");
            }

            // 4?? ������� ������������ �� ����� �������
            var reservation = new ReservationRequest
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = request.ReservationTime,
                EmployeeId = DefaultEmployeeId, // ������ ID = 1
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);

            // 5?? ������ ������ ����� �� "reserved"
            table.Status = "reserved";
            _context.Tables.Update(table);

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
}