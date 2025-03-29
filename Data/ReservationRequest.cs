using System.Text.Json.Serialization;
using MyRestoranApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

[Table("reservations")] // Указываем имя таблицы в базе данных
public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }

    // Используем DateTime вместо DateTimeOffset для хранения времени без временной зоны
    public DateTime ReservationTime { get; set; } // Время без временной зоны

    public int EmployeeId { get; set; }

    // Устанавливаем время создания без временной зоны, с использованием UTC
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Время в UTC без временной зоны

    public string Status { get; set; } = "Reserved"; // Всегда "Reserved"
}
