using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("reservations")]
    public class Reservation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("customer_id")]
        [Required]
        [ForeignKey("Customer")] // Добавляем ForeignKey
        public int CustomerId { get; set; }

        [Column("table_id")]
        [Required]
        [ForeignKey("Table")] // Добавляем ForeignKey
        public int TableId { get; set; }

        [Column("reservation_time")]
        [Required]
        public DateTime ReservationTime { get; set; }

        [Column("employee_id")]
        [Required]
        public int EmployeeId { get; set; } = 1; // Всегда 1

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Customer Customer { get; set; }
        public Table Table { get; set; }
    }
}
