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

            // Создаем заказ
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
                EmployeeId = null // Можно назначить, если необходимо
            };

            // Добавляем заказ в контекст
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();  // Сначала сохраняем заказ, чтобы получить его ID

            // Создаем элементы для заказа
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    DishId = item.DishId,
                    Quantity = item.Quantity,
                    OrderId = order.Id  // Привязываем элемент к заказу
                };

                // Добавляем элемент в контекст
                _context.OrderItems.Add(orderItem);
            }

            // Сохраняем изменения для элементов заказа
            await _context.SaveChangesAsync();

            // Возвращаем успешный ответ с данными созданного заказа
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
