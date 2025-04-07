using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("customers")] 
    public class Customer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Column("last_name")]
        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        [Column("phone")]
        [MaxLength(20)]
        public required string Phone { get; set; }
    }
}
