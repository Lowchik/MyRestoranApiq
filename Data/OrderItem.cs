using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRestoranApi.Data
{
    [Table("order_items")] // ��������� �������� ������� � ���� ������
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; } // ������������� �������� ������

        [Column("order_id")]
        [ForeignKey("Order")]
        public int OrderId { get; set; } // ������������� ������ (����� � �������� Order)

        [Column("dish_id")]
        [ForeignKey("Dish")]
        public int DishId { get; set; } // ������������� ����� (����� � �������� Dish)

        [Column("quantity")]
        public int Quantity { get; set; } // ���������� ���� � ������

        
        public Order? Order { get; set; } // ����� � ������� (Order)
        public Dish? Dish { get; set; } // ����� � ������ (Dish)
    }
}
