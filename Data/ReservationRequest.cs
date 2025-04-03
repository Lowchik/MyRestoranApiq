using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("reservations")] // ”казываем правильное им€ таблицы в базе данных
    public class Reservation
    {
        [Key]
        [Column("id")] 
        public int Id { get; set; }

        [Required] 
        [ForeignKey("Customer")] 
        [Column("customer_id")] 
        public int CustomerId { get; set; }

        [Required] 
        [ForeignKey("Table")] 
        [Column("table_id")] 
        public int TableId { get; set; }

        [Required] 
        [Column("reservation_time")] 
        public DateTime ReservationTime { get; set; } = DateTime.UtcNow;

        [ForeignKey("Employee")] 
        [Column("employee_id")] 
        public int? EmployeeId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required] 
        [MaxLength(50)] 
        [Column("status")]
        public string Status { get; set; } = "Reserved"; 

        [Required]
        [Column("end_time")] 
        public DateTime EndTime { get; set; } = DateTime.UtcNow;

    }
}
