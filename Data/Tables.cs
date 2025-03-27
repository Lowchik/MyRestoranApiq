using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("tables")] // Указываем имя таблицы
    public class Table
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("table_number")]
        [Required]
        public int TableNumber { get; set; } // Номер стола

        [Column("seats")]
        [Required]
        public int Seats { get; set; } // Количество мест

        [Column("status")]
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "available"; // Статус стола (по умолчанию "available")
    }
}
