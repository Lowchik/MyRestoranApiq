using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("customers")] // Указываем правильное имя таблицы
    public class Customer
    {
        [Key]
        [Column("id")] // Убедитесь, что здесь указано правильное имя столбца
        public int Id { get; set; } // Поле id, будет автоинкрементироваться

        [Column("first_name")] // Соответствует полю first_name в базе
        [Required] // Убедимся, что поле обязательно для заполнения
        [MaxLength(100)] // Устанавливаем максимальную длину строки
        public string FirstName { get; set; }

        [Column("last_name")] // Соответствует полю last_name в базе
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Column("phone")] // Соответствует полю phone в базе
        [MaxLength(20)] // Максимальная длина, как указано в базе
        public string Phone { get; set; }
    }
}
