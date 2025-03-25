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

    // GET api/customers
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] string? phone)
    {
        try
        {
            IQueryable<Customer> query = _context.Customers;

            // ���� ������� ������� � �������, ��������� �� ������ ��������
            if (!string.IsNullOrEmpty(phone))
            {
                // ������� ��� ���������� �������, �� ��������� ������� +
                var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());

                // ��������, ��� ���������� ����� ��������������
                Console.WriteLine($"Formatted phone: {formattedPhone}");

                // ���������� Equals ��� ������� ����������
                query = query.Where(c => c.Phone.Equals(formattedPhone));
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

    // GET api/customers/exists
    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists([FromQuery] string phone)
    {
        try
        {
            // ������� ��� ���������� �������, �� ��������� ������� +
            var formattedPhone = new string(phone.Where(char.IsDigit).ToArray());

            // ��������, ��� ���������� ����� ��������������
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            // ���������� Equals ��� ������� ����������
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone.Equals(formattedPhone));

            if (customer != null)
            {
                return Ok(true); // ������
            }

            return Ok(false); // �� ������
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
