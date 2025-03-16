using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/customers
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        try
        {
            var customers = await _context.Customers.ToListAsync();
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

    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists(string phone)
    {
        // Убираем все пробелы и символ "+" из номера телефона
        var formattedPhone = phone.Trim().Replace(" ", "").TrimStart('+');

        // Выводим, что получилось после форматирования
        Console.WriteLine($"Formatted phone: {formattedPhone}");

        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Phone.Replace(" ", "").TrimStart('+') == formattedPhone);

        if (customer != null)
        {
            return Ok(true); // Найден
        }

        return Ok(false); // Не найден
    }


}
