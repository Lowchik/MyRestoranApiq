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

    // GET api/customers?phone=+79001234567
    [HttpGet]
    public async Task<IActionResult> GetCustomerByPhone([FromQuery] string? phone)
    {
         Console.WriteLine($"[GetCustomerByPhone]��������� � ����� �: {phone}");


        if (string.IsNullOrEmpty(phone))
        {
            return BadRequest("[GetCustomerByPhone]����� �������� ����������.");
        }

        try
        {
            // ����������� ����� ��������
            var formattedPhone = FormatPhoneNumber(phone);
            Console.WriteLine($"[GetCustomerByPhone]Formatted phone: {formattedPhone}");

            // ���� ������ �������
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => EF.Functions.Like(c.Phone, $"%{formattedPhone}%"));

            if (customer == null)
            {
                Console.WriteLine("[GetCustomerByPhone]������ �� ������.");
                return NotFound("[GetCustomerByPhone]������ �� ������.");
            }

            Console.WriteLine($"[GetCustomerByPhone]���������� �������: {customer.FirstName} {customer.LastName}");
            return Ok(customer);
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
            return CreatedAtAction(nameof(GetCustomerByPhone), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET api/customers/exists
    [HttpGet("exists")]
    public async Task<IActionResult> PhoneExists(string phone)
    {
        try
        {
            // ����������� ����� ��������
            var formattedPhone = FormatPhoneNumber(phone);

            // ��������, ��� ���������� ����� ��������������
            Console.WriteLine($"Formatted phone: {formattedPhone}");

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => EF.Functions.Like(c.Phone, $"%{formattedPhone}%"));

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

    // ����� ��� ����������� �������������� ������ ��������
    private string FormatPhoneNumber(string phone)
    {
        // ������� ��� ���������� �������, �� ��������� ������� +
        var formattedPhone = new string(phone.Where(c => char.IsDigit(c) || c == '+').ToArray());
        return formattedPhone;
    }
}
