using System.Text.Json.Serialization;
using MyRestoranApi.Data;

public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int EmployeeId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Reserved"; // Всегда "Reserved"

    [JsonIgnore] // Исключаем из JSON, чтобы не требовалось в запросе
    public Customer Customer { get; set; }

    [JsonIgnore]
    public Table Table { get; set; }
}
