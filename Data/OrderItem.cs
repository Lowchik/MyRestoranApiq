using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        [Required]
        public int OrderId { get; set; }

        [Column("dish_id")]
        [Required]
        public int DishId { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }

        public Order? Order { get; set; }
        public Dish? Dish { get; set; }
    }
}
