using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

namespace MyRestoranApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderTypeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderType>>> GetOrderTypes()
        {
            return await _context.OrderTypes.ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderType>> GetOrderType(int id)
        {
            var orderType = await _context.OrderTypes.FindAsync(id);

            if (orderType == null)
            {
                return NotFound();
            }

            return orderType;
        }
    }
}
