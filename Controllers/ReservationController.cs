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
    private const int DefaultEmployeeId = 1; // Всегда назначаем ID сотрудника = 1

    public ReservationController(AppDbContext context, ILogger<ReservationController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Создание бронирования
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation request)
    {
        _logger.LogInformation($"Запрос на бронирование: Customer {request.CustomerId}, Table {request.TableId}, Time {request.ReservationTime}");

        try
        {
            // Проверяем, существует ли стол
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
            {
                _logger.LogWarning("Стол не найден.");
                return NotFound("Стол не найден.");
            }

            // Проверяем, нет ли пересечений бронирования на это время
            var overlappingReservation = await _context.Reservations
                .AnyAsync(r => r.TableId == request.TableId && r.ReservationTime == request.ReservationTime);

            if (overlappingReservation)
            {
                _logger.LogWarning("Этот стол уже забронирован на это время.");
                return BadRequest("Этот стол уже забронирован на это время.");
            }

            // Создаем бронирование
            var reservation = new Reservation
            {
                CustomerId = request.CustomerId,
                TableId = request.TableId,
                ReservationTime = request.ReservationTime,
                EmployeeId = DefaultEmployeeId, // Всегда ID = 1
                CreatedAt = DateTime.UtcNow,
                Status = Reservation.ReservationStatus.Reserved // Устанавливаем статус "Reserved"
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Бронирование успешно создано для клиента {request.CustomerId}, стол {request.TableId}.");

            return Ok(new
            {
                message = "Стол успешно забронирован!",
                reservationId = reservation.Id,
                employeeId = DefaultEmployeeId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка сервера: {ex.Message}");
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    // Получить список всех бронирований
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
            _logger.LogError($"Ошибка при получении бронирований: {ex.Message}");
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }

    // Получить список всех столов
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
            _logger.LogError($"Ошибка при получении столов: {ex.Message}");
            return StatusCode(500, $"Ошибка сервера: {ex.Message}");
        }
    }
}
