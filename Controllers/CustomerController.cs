using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // Метод для форматирования телефона
    private string FormatPhone(string phone)
    {
        // Логирование перед форматированием
        Console.WriteLine($"Original phone: {phone}");

        // Оставляем только цифры, но сохраняем префикс +
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        var formattedPhone = phone.StartsWith("+") ? "+" + digits : digits;

        // Логирование после форматирования
        Console.WriteLine($"Formatted phone: {formattedPhone}");
        return formattedPhone;
    }



    // GET api/customers
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] string? phone)
    {
        try
        {
            IQueryable<Customer> query = _context.Customers;

            // Если передан телефон в запросе, фильтруем по номеру телефона
            if (!string.IsNullOrEmpty(phone))
            {
                var formattedPhone = FormatPhone(phone); // Используем общий метод для форматирования

                // Логируем, что получилось после форматирования
                Console.WriteLine($"Formatted phone: {formattedPhone}");

                // Фильтруем по номеру телефона с точным совпадением
                query = query.Where(c => c.Phone == formattedPhone); // Используем == вместо Contains
            }

            var customers = await query.ToListAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST api/customers
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

    // GET api/customers/exists?phone=
    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists(string phone)
    {
        try
        {
            var formattedPhone = FormatPhone(phone); // Используем общий метод для форматирования

            // Логируем, что получилось после форматирования
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            // Ищем пользователя с точным совпадением по телефону
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == formattedPhone); // Используем == вместо Contains

            if (customer != null)
            {
                return Ok(true); // Найден
            }

            return Ok(false); // Не найден
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
