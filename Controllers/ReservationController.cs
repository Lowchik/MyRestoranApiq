using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private const int DefaultEmployeeId = 1; 

    public ReservationController(AppDbContext context)
    {
        _context = context;
        Console.WriteLine("Create controller");
    }
    [HttpPost("reservation")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        Console.WriteLine($"Запрос на бронирование: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime} до {request.EndTime}");

        try
        {
            
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                Console.WriteLine("Клиент не найден.");
                return NotFound(new { message = "Клиент не найден." });
            }

      
            var tableExists = await _context.Tables.AnyAsync(t => t.Id == request.TableId);
            if (!tableExists)
            {
                Console.WriteLine("Стол не найден.");
                return NotFound(new { message = "Стол не найден." });
            }

           
            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = request.ReservationTime, 
                EndTime = request.EndTime,
                EmployeeId = DefaultEmployeeId,
                Status = "Reserved"
            };

            _context.Reservations.Add(reservation);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                return StatusCode(500, new { message = "Ошибка сервера", error = ex.Message });
            }

            Console.WriteLine($"Бронирование создано: Клиент {request.CustomerId}, стол {request.TableId}, c {reservation.ReservationTime} до {reservation.EndTime}.");

            return Ok(new
            {
                message = "Бронирование успешно создано!",
                reservationId = reservation.Id
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сервера: {ex.Message}");
            return StatusCode(500, new { message = "Ошибка сервера", error = ex.Message });
        }
    }





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
            Console.WriteLine($"Ошибка при получении бронирований: {ex.Message}");
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

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
            Console.WriteLine($"Ошибка при получении столов: {ex.Message}");
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }
}
