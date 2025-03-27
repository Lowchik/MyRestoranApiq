using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("tables")] // ��������� ��� �������
    public class Table
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("table_number")]
        [Required]
        public int TableNumber { get; set; } // ����� �����

        [Column("seats")]
        [Required]
        public int Seats { get; set; } // ���������� ����

        [Column("status")]
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "available"; // ������ ����� (�� ��������� "available")
    }
}
