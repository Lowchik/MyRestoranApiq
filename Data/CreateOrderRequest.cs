using System.ComponentModel.DataAnnotations;

namespace MyRestoranApi.Dto
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public int OrderTypeId { get; set; }
        public int? CourierId { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public List<OrderItems> Items { get; set; } = new();
    }

    public class OrderItems
    {
        public int DishId { get; set; }
        public int Quantity { get; set; }
    }


}
