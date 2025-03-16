using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("customers")] // ��������� ���������� ��� �������
    public class Customer
    {
        [Key]
        [Column("id")] // ���������, ��� ����� ������� ���������� ��� �������
        public int Id { get; set; } // ���� id, ����� ����������������������

        [Column("first_name")] // ������������� ���� first_name � ����
        [Required] // ��������, ��� ���� ����������� ��� ����������
        [MaxLength(100)] // ������������� ������������ ����� ������
        public string FirstName { get; set; }

        [Column("last_name")] // ������������� ���� last_name � ����
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Column("phone")] // ������������� ���� phone � ����
        [MaxLength(20)] // ������������ �����, ��� ������� � ����
        public string Phone { get; set; }
    }
}
