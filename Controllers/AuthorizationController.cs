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

            var hashedPassword = HashPassword(request.Password);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                return Unauthorized("Nepravinyy email ili parol.");
            }

            if (user.Role == null)
            {
                return Unauthorized("Rol polzovatelya ne naydena.");
            }

            var roleName = user.Role.Name?.Trim();

            if (!string.Equals(roleName?.Normalize(), "courier".Normalize(), StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized($"Доступ разрешён только курьерам. Роль пользователя: '{roleName}'");
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


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); 
                }

                return builder.ToString();
            }
        }
    }
}
