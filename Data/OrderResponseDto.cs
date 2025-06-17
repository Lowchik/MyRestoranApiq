public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderTime { get; set; }
    public string? DeliveryAddress { get; set; }
    public decimal? TotalPrice { get; set; }

    public string? CustomerName { get; set; }
    public string? CourierName { get; set; }
    public string? StatusName { get; set; }
    public string? OrderTypeName { get; set; }

    public int StatusId { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public string? DishName { get; set; }
    public int Quantity { get; set; }
}
