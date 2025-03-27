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
    private const int DefaultEmployeeId = 1; // Всегда назначаем ID сотрудника = 1

    public ReservationController(AppDbContext context)
    {
        _context = context;
    }

    // ? Создание бронирования
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
    {
        Console.WriteLine($"Запрос на бронирование: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // 1?? Проверяем, существует ли стол
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                return NotFound("Стол не найден.");
            }

            // 2?? Проверяем, свободен ли стол
            if (table.Status != "available")
            {
                return BadRequest("Этот стол уже забронирован.");
            }

            // 3?? Проверяем, нет ли пересечений бронирования на это время
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
                return BadRequest("Этот стол уже забронирован на это время.");
            }

            // 4?? Создаем бронирование со всеми данными
            var reservation = new ReservationRequest
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = request.ReservationTime,
                EmployeeId = DefaultEmployeeId, // Всегда ID = 1
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);

            // 5?? Меняем статус стола на "reserved"
            table.Status = "reserved";
            _context.Tables.Update(table);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Стол успешно забронирован!",
                reservationId = reservation.Id,
                employeeId = DefaultEmployeeId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    // ? Получить список всех бронирований
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