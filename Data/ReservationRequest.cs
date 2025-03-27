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
        [ForeignKey("Customer")] // ����� � �������� customers
        public int CustomerId { get; set; }

        [Column("table_id")]
        [Required]
        [ForeignKey("Table")] // ����� � �������� tables
        public int TableId { get; set; }

        [Column("reservation_time")]
        [Required]
        public DateTime ReservationTime { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; } // ������ ����� ���� NULL

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("status")]
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = ReservationStatus.Available.ToString();
        public enum ReservationStatus
        {
            Available,  // �������� (�� ���������)
            Reserved,   // �������������
            Completed,  // ���������
            Cancelled   // ��������
        }

        public Customer Customer { get; set; }
        public Table Table { get; set; }
    }
}
