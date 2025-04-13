using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [ForeignKey("OrderId")]
        [JsonIgnore]  
        public Order? Order { get; set; }

        [Column("dish_id")]
        [Required]
        public int DishId { get; set; }

        [ForeignKey("DishId")]
        public Dish? Dish { get; set; }

        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }
    }
}
