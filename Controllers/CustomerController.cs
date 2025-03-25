using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System.Linq;

[ApiController]
[Route("api/customer")] // Изменили здесь, теперь маршрут будет '/api/customer'
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/customer?phone=+79995556677
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] string phone)
    {
        try
        {
            // Если передан телефон в запросе, фильтруем по номеру телефона
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest("Phone number is required.");
            }

            // Убираем все нецифровые символы, но сохраняем префикс "+"
            var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());

            // Логируем, что получилось после форматирования
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            // Фильтруем клиентов по номеру телефона
            var customers = await _context.Customers
                .Where(c => c.Phone.Contains(formattedPhone))
                .ToListAsync();

            if (customers.Count == 0)
            {
                return NotFound("No customers found with this phone number.");
            }

            return Ok(customers); // Возвращаем список клиентов
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST api/customer
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET api/customer/exists?phone=+79995556677
    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists([FromQuery] string phone)
    {
        try
        {
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest("Phone number is required.");
            }

            // Убираем все нецифровые символы, но сохраняем префикс "+"
            var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());

            // Логируем, что получилось после форматирования
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            // Ищем клиента с данным номером телефона
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone.Contains(formattedPhone));

            if (customer != null)
            {
                return Ok(true); // Клиент найден
            }

            return Ok(false); // Клиент не найден
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
