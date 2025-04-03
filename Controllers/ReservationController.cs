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

        Console.WriteLine($"Запрос на бронирование: Customer {request.CustomerId}, Table {request.TableId}, Time start {request.ReservationTime} Time end {request.EndTime}");

        try
        {
            // Проверяем, существует ли клиент
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == request.CustomerId);
            if (!customerExists)
            {
                Console.WriteLine("Клиент не найден.");
                return NotFound(new { message = "Client not found." });
            }

            // Проверяем, существует ли стол
            var tableExists = await _context.Tables.AnyAsync(t => t.Id == request.TableId);
            if (!tableExists)
            {
                Console.WriteLine("Стол не найден.");
                return NotFound(new { message = "Table not found." });
            }

            
            var reservationTime = (request.ReservationTime).ToUniversalTime();
            var endTime = (request.EndTime).ToUniversalTime();

            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = reservationTime,
                EndTime = endTime,
                EmployeeId = DefaultEmployeeId,
                Status = "Reserved"
            };

            // Добавляем бронирование в базу данных
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
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }

            Console.WriteLine($"Бронирование создано: Клиент {request.CustomerId}, Стол {request.TableId}.");

            return Ok(new
            {
                message = "Бронирование успешно создано!",
                reservationId = reservation.Id
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сервера: {ex.Message}");
            return StatusCode(500, new { message = "Server error", error = ex.Message });
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

    [HttpGet("reservations/forDate")]
    public async Task<IActionResult> GetReservationsForDate([FromQuery] string date, [FromQuery] string startTime, [FromQuery] string endTime)
    {
        try
        {
            
            var reservationDate = DateTime.Parse(date).ToUniversalTime();
            var startDateTime = DateTime.Parse($"{date} {startTime}").ToUniversalTime();
            var endDateTime = DateTime.Parse($"{date} {endTime}").ToUniversalTime();

            // Получаем все бронирования на эту дату и время
            var reservations = await _context.Reservations
                .Where(r => r.ReservationTime < endDateTime && r.EndTime > startDateTime)
                .ToListAsync();

            // Возвращаем список забронированных столов
            var reservedTableIds = reservations.Select(r => r.TableId).ToList();

            return Ok(reservedTableIds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении бронирований: {ex.Message}");
            return StatusCode(500, new { message = "Server error", error = ex.Message });
        }
    }
}
