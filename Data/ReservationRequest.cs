using System.Text.Json.Serialization;
using MyRestoranApi.Data;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }
    public DateTimeOffset ReservationTime { get; set; } // Используем DateTimeOffset для учета временной зоны
    public int EmployeeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; // Убедитесь, что время в UTC
    public string Status { get; set; } = "Reserved"; // Всегда "Reserved"
}

