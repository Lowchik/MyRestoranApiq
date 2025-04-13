using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("order_items")] // Указываем название таблицы в базе данных
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // Идентификатор элемента заказа

        [Column("order_id")]
        [ForeignKey("Order")]
        public int OrderId { get; set; } // Идентификатор заказа (связь с таблицей Order)

        [Column("dish_id")]
        [ForeignKey("Dish")]
        public int DishId { get; set; } // Идентификатор блюда (связь с таблицей Dish)

        [Column("quantity")]
        public int Quantity { get; set; } // Количество блюд в заказе

        
        public Order? Order { get; set; } // Связь с заказом (Order)
        public Dish? Dish { get; set; } // Связь с блюдом (Dish)
    }
}
