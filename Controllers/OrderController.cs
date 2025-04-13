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
            // Проверка на правильность данных
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
            {
                return BadRequest("Invalid order data.");
            }

           
            var order = new Order
            {
                CustomerId = request.CustomerId,
                StatusId = request.StatusId,
                OrderTypeId = request.OrderTypeId,
                EmployeeId = null, 
                CourierId = request.CourierId,
                DeliveryAddress = request.DeliveryAddress,
                TotalPrice = request.TotalPrice,
                OrderTime = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };


            _context.Orders.Add(order);
            await _context.SaveChangesAsync();  

           
            foreach (var orderItemDto in request.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    DishId = orderItemDto.DishId,
                    Quantity = orderItemDto.Quantity,
                };

                _context.OrderItems.Add(orderItem);
            }

            // Сохранение заказанных блюд
            await _context.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Dish)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
