using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System.Linq;

[ApiController]
[Route("api/controller")] // �������� ���� �� '/api/controller'
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/controller?phone=+79995556677
    // GET api/controller
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] string phone)
    {
        try
        {
            if (!string.IsNullOrEmpty(phone))
            {
                // ���� ������� ������� � �������, ��������� �� ������ ��������
                var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());
                var customers = await _context.Customers
                    .Where(c => c.Phone.Contains(formattedPhone))
                    .ToListAsync();

                if (customers.Count == 0)
                {
                    return NotFound("No customers found with this phone number.");
                }

                return Ok(customers); // ���������� ������ ��������
            }
            else
            {
                // ���� ������� �� �������, ���������� ���� ��������
                var customers = await _context.Customers.ToListAsync();

                if (customers.Count == 0)
                {
                    return NotFound("No customers found.");
                }

                return Ok(customers); // ���������� ���� ��������
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST api/controller
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

    // GET api/controller/exists?phone=+79995556677
    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists([FromQuery] string phone)
    {
        try
        {
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest("Phone number is required.");
            }

            var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone.Contains(formattedPhone));

            if (customer != null)
            {
                return Ok(true); // ������ ������
            }

            return Ok(false); // ������ �� ������
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
