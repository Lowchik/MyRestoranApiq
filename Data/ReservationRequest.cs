using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("reservations")] // Указываем правильное имя таблицы в базе данных
    public class Reservation
    {
        [Key] // Указываем, что это основной ключ
        [Column("id")] // Указываем имя колонки в базе данных
        public int Id { get; set; }

        [Required] // Обязательно для заполнения
        [ForeignKey("Customer")] // Связь с таблицей customers
        [Column("customer_id")] // Указываем имя колонки в базе данных
        public int CustomerId { get; set; }

        [Required] // Обязательно для заполнения
        [ForeignKey("Table")] // Связь с таблицей tables
        [Column("table_id")] // Указываем имя колонки в базе данных
        public int TableId { get; set; }

        [Required] // Время бронирования обязательно
        [Column("reservation_time")] // Указываем имя колонки в базе данных
        public DateTime ReservationTime { get; set; }

        [ForeignKey("Employee")] // Связь с таблицей employees
        [Column("employee_id")] // Указываем имя колонки в базе данных
        public int? EmployeeId { get; set; } // Nullable, так как не всегда может быть указан

        [Column("created_at")] // Указываем имя колонки в базе данных
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Время по умолчанию в UTC

        [Required] // Статус всегда должен быть указан
        [MaxLength(50)] // Максимальная длина строки
        [Column("status")] // Указываем имя колонки в базе данных
        public string Status { get; set; } = "Reserved"; // Статус по умолчанию
    }
}
