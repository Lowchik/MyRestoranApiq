using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("dishes")]
    public class Dish
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [ForeignKey("Category")]
        [Column("category_id")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
