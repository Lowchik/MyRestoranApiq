using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System.Security.Cryptography;
using System.Text;

namespace MyRestoranApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorizationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email и пароль обязательны.");
            }

            var hashedPassword = HashPasswordSha256(request.Password);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                return Unauthorized("Nepravil'nyy email ili parol'.");
            }

            if (user.Role == null)
            {
                return Unauthorized("Rol' pol'zovatelya ne naydena.");
            }

            var roleName = user.Role.Name?.Trim();

            if (!string.Equals(roleName, "Курьер", StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized($"Dostup razreshyon tol'ko kur'yeram. Rol' pol'zovatelya: '{roleName}'");
            }



            var courier = await _context.Couriers
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (courier == null)
            {
                return NotFound("courir ne nayden.");
            }

            return Ok(new
            {
                message = "vxod good",
                userId = user.Id,
                role = user.Role.Name,
                email = user.Email,
                courier = new
                {
                    courierId = courier.Id,
                    firstName = courier.FirstName,
                    lastName = courier.LastName,
                    phone = courier.Phone,
                    vehicleType = courier.VehicleType
                }
            });
        }


        private string HashPasswordSha256(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
