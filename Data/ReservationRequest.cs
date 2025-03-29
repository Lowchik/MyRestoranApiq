using System.Text.Json.Serialization;
using MyRestoranApi.Data;
using System.ComponentModel.DataAnnotations.Schema;

[Table("reservations")] // ��������� ��� ������� � ���� ������
public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }

    // ���������� DateTime ������ DateTimeOffset ��� �������� ������� ��� ��������� ����
    public DateTime ReservationTime { get; set; } // ����� ��� ��������� ����

    public int EmployeeId { get; set; }

    // ������������� ����� �������� ��� ��������� ����, � �������������� UTC
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ����� � UTC ��� ��������� ����

    public string Status { get; set; } = "Reserved"; // ������ "Reserved"
}
