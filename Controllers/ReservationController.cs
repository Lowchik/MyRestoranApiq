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
        Console.WriteLine("Create controller");
    }

    [HttpPost("reservation")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        Console.WriteLine($"������ �� ������������: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // ��������, ���������� �� ������
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                Console.WriteLine("������ �� ������.");
                return NotFound(new { message = "������ �� ������." });
            }

            // ���������, ���������� �� ����
            var tableExists = await _context.Tables.AnyAsync(t => t.Id == request.TableId);
            if (!tableExists)
            {
                Console.WriteLine("���� �� ������.");
                return NotFound(new { message = "���� �� ������." });
            }

            // ������ ��������� ����� ������������ ��� ���������� ��������
            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = DateTime.UtcNow,  // ������������� ����� ��� ��������� ����
                EmployeeId = DefaultEmployeeId,
                Status = "Reserved"
            };

            _context.Reservations.Add(reservation);

            // �������� ����������
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // ��������� ����������
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ���������� ���������� � ������ ������
                Console.WriteLine($"������ ��� ����������: {ex.Message}");
                Console.WriteLine($"��������� ����������: {ex.InnerException?.Message}");
                return StatusCode(500, new { message = "������ �������", error = ex.Message });
            }

            Console.WriteLine($"������������ �������: ������ {request.CustomerId}, ���� {request.TableId}.");

            return Ok(new
            {
                message = "������������ ������� �������!",
                reservationId = reservation.Id
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"������ �������: {ex.Message}");
            return StatusCode(500, new { message = "������ �������", error = ex.Message });
        }
    }


    // �������� ������ ���� ������������
    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        try
        {
            var reservations = await _context.Reservations.ToListAsync();

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
