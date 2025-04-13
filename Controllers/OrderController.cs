using Microsoft.AspNetCore.Mvc;
using MyRestoranApi.Data;
using MyRestoranApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace MyRestoranApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
            {
                return BadRequest("Invalid order data.");
            }

        
            var order = new Order
            {
                CustomerId = request.CustomerId,
                StatusId = request.StatusId,
                OrderTypeId = request.OrderTypeId,
                CourierId = request.CourierId,
                DeliveryAddress = request.DeliveryAddress,
                TotalPrice = request.TotalPrice,
                OrderTime = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmployeeId = null 
            };

           
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); 

           
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    OrderId = order.Id  
                };

               
                _context.OrderItems.Add(orderItem);
            }

           
            await _context.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .Include(o => o.Status)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    OrderTime = o.OrderTime,
                    DeliveryAddress = o.DeliveryAddress,
                    TotalPrice = o.TotalPrice,
                    CustomerName = o.Customer != null
                        ? o.Customer.FirstName + " " + o.Customer.LastName
                        : "����������� ������",
                    CourierName = o.Courier != null
                        ? o.Courier.FirstName + " " + o.Courier.LastName
                        : "��� �������",
                    StatusName = o.Status != null
                        ? o.Status.Name
                        : "����������� ������",
                    OrderTypeName = o.OrderType != null
                        ? o.OrderType.Name
                        : "����������� ��� ������",
                    Items = o.OrderItems.Select(i => new OrderItemDto
                    {
                        DishName = i.Dish != null ? i.Dish.Name : "����������� �����",
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


    }
}
