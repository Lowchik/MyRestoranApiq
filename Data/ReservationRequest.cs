using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("reservations")] // ��������� ���������� ��� ������� � ���� ������
    public class Reservation
    {
        [Key] // ���������, ��� ��� �������� ����
        [Column("id")] // ��������� ��� ������� � ���� ������
        public int Id { get; set; }

        [Required] // ����������� ��� ����������
        [ForeignKey("Customer")] // ����� � �������� customers
        [Column("customer_id")] // ��������� ��� ������� � ���� ������
        public int CustomerId { get; set; }

        [Required] // ����������� ��� ����������
        [ForeignKey("Table")] // ����� � �������� tables
        [Column("table_id")] // ��������� ��� ������� � ���� ������
        public int TableId { get; set; }

        [Required] // ����� ������������ �����������
        [Column("reservation_time")] // ��������� ��� ������� � ���� ������
        public DateTime ReservationTime { get; set; }

        [ForeignKey("Employee")] // ����� � �������� employees
        [Column("employee_id")] // ��������� ��� ������� � ���� ������
        public int? EmployeeId { get; set; } // Nullable, ��� ��� �� ������ ����� ���� ������

        [Column("created_at")] // ��������� ��� ������� � ���� ������
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ����� �� ��������� � UTC

        [Required] // ������ ������ ������ ���� ������
        [MaxLength(50)] // ������������ ����� ������
        [Column("status")] // ��������� ��� ������� � ���� ������
        public string Status { get; set; } = "Reserved"; // ������ �� ���������
    }
}
