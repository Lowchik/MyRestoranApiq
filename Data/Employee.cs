using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("employees")] // Указываем правильное имя таблицы
    public class Employee
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; } 

        [Column("position")]
        [Required]
        [MaxLength(50)]
        public string Position { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Column("phone")]
        [MaxLength(20)]
        public string? Phone { get; set; } // Может быть пустым
        public Employee(int userId, string position, string firstName, string lastName)
        {
            UserId = userId;
            Position = position;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
