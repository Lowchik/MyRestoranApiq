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
                        : "Неизвестный клиент",
                    CourierName = o.Courier != null
                        ? o.Courier.FirstName + " " + o.Courier.LastName
                        : "Без курьера",
                    StatusName = o.Status != null
                        ? o.Status.Name
                        : "Неизвестный статус",
                    OrderTypeName = o.OrderType != null
                        ? o.OrderType.Name
                        : "Неизвестный тип заказа",
                    Items = o.OrderItems.Select(i => new OrderItemDto
                    {
                        DishName = i.Dish != null ? i.Dish.Name : "Неизвестное блюдо",
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

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Status)
                .Include(o => o.OrderType)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Dish)
                .Select(o => new OrderResponseDto
                {
                   
                    OrderTime = o.OrderTime,
                    DeliveryAddress = o.DeliveryAddress,
                    TotalPrice = o.TotalPrice,
                    CourierName = o.Courier != null
                        ? o.Courier.FirstName + " " + o.Courier.LastName
                        : "Без курьера",
                    StatusName = o.Status != null
                        ? o.Status.Name
                        : "Неизвестный статус",
                    OrderTypeName = o.OrderType != null
                        ? o.OrderType.Name
                        : "Неизвестный тип заказа",
                    Items = o.OrderItems.Select(i => new OrderItemDto
                    {
                        DishName = i.Dish != null ? i.Dish.Name : "Неизвестное блюдо",
                        Quantity = i.Quantity
                    }).ToList()
                })
                .OrderByDescending(o => o.OrderTime) 
                .ToListAsync();

            return Ok(orders);
        }
        [HttpGet("courier/orders/pending")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetPendingOrders()
        {
            var pendingStatusId = 1;

            var orders = await _context.Orders
                .Where(o => o.StatusId == pendingStatusId)
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
                        : "Неизвестный клиент",

                    CourierName = o.Courier != null
                        ? o.Courier.FirstName + " " + o.Courier.LastName
                        : null,

                    StatusId = o.StatusId,  

                    StatusName = o.Status != null
                        ? o.Status.Name
                        : null,

                    OrderTypeName = o.OrderType != null
                        ? o.OrderType.Name
                        : null,

                    Items = o.OrderItems.Select(i => new OrderItemDto
                    {
                        DishName = i.Dish != null ? i.Dish.Name : "Неизвестное блюдо",
                        Quantity = i.Quantity
                    }).ToList()
                })
                .OrderBy(o => o.OrderTime)
                .ToListAsync();

            return Ok(orders);
        }


        [HttpPatch("{orderId}/assign-courier")]
        public async Task<IActionResult> AssignCourier(int orderId, [FromBody] AssignCourierRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            if (order.StatusId != 1)
                return BadRequest("Заказ уже обработан или в другом статусе.");

            order.CourierId = request.CourierId;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{orderId}/update-status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound();

            
            if (order.CourierId != request.CourierId)
                return BadRequest("Вы не назначены на этот заказ.");

          
            order.StatusId = request.StatusId;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        public class AssignCourierRequest
        {
            public int CourierId { get; set; }
        }

    }
}
