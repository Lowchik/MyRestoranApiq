using System.ComponentModel.DataAnnotations;

namespace MyRestoranApi.Dto
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public int OrderTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CourierId { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } // Список блюд
    }

    public class OrderItemDto
    {
        public int DishId { get; set; }
        public int Quantity { get; set; } // Количество каждого блюда
    }
}
