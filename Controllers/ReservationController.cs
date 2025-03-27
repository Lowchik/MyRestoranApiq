using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private const int DefaultEmployeeId = 1; // ������ ��������� ID ���������� = 1

    public ReservationController(AppDbContext context)
    {
        _context = context;
        Console.WriteLine("������ ���������");
    }

    // �������� ������������
    [HttpPost("reservation")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        Console.WriteLine($"������ �� ������������: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // �������� �� ������������� �������
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                Console.WriteLine("������ �� ������.");
                return NotFound("������ �� ������.");
            }

            // ���������, ���������� �� ����
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                Console.WriteLine("���� �� ������.");
                return NotFound("���� �� ������.");
            }

            // �������� �� ������������� ����������
            var employeeExists = await _context.Employee.AnyAsync(e => e.Id == DefaultEmployeeId);
            if (!employeeExists)
            {
                Console.WriteLine("��������� �� ������.");
                return NotFound("��������� �� ������.");
            }

            // ���������, ��� �� ����������� ������������ �� ��� �����
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
                Console.WriteLine("���� ���� ��� ������������ �� ��� �����.");
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

            Console.WriteLine($"������������ ������� ������� ��� ������� {request.CustomerId}, ���� {request.TableId}.");

            return Ok(new
            {
                message = "���� ������� ������������!",
                reservationId = reservation.Id,
                employeeId = DefaultEmployeeId
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ �������: {ex.Message}");
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
            Console.WriteLine($"������ ��� ��������� ������������: {ex.Message}");
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
            Console.WriteLine($"������ ��� ��������� ������: {ex.Message}");
            return StatusCode(500, $"������ �������: {ex.Message}");
        }
    }
}
