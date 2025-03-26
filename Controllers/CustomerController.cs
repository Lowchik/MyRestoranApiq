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

    // ����� ��� �������������� ��������
    private string FormatPhone(string phone)
    {
        // ����������� ����� ���������������
        Console.WriteLine($"Original phone: {phone}");

        // ��������� ������ �����, �� ��������� ������� +
        var digits = new string(phone.Where(char.IsDigit).ToArray());
        var formattedPhone = phone.StartsWith("+") ? "+" + digits : digits;

        // ����������� ����� ��������������
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

            // ���� ������� ������� � �������, ��������� �� ������ ��������
            if (!string.IsNullOrEmpty(phone))
            {
                var formattedPhone = FormatPhone(phone); // ���������� ����� ����� ��� ��������������

                // ��������, ��� ���������� ����� ��������������
                Console.WriteLine($"Formatted phone: {formattedPhone}");

                // ��������� �� ������ �������� � ������ �����������
                query = query.Where(c => c.Phone == formattedPhone); // ���������� == ������ Contains
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
            var formattedPhone = FormatPhone(phone); // ���������� ����� ����� ��� ��������������

            // ��������, ��� ���������� ����� ��������������
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            // ���� ������������ � ������ ����������� �� ��������
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == formattedPhone); // ���������� == ������ Contains

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
