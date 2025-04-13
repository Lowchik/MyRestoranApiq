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
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.CustomerId,
                    o.OrderTime,
                    o.StatusId,
                    o.OrderTypeId,
                    o.EmployeeId,
                    o.UpdatedAt,
                    o.TotalPrice,
                    o.CourierId,
                    o.DeliveryAddress
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
