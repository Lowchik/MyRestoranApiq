using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestoranApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        private readonly AppDbContext _context;

        public ImageController(AppDbContext context)
        {
            _context = context;

            // Создаем папку, если она не существует
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        [HttpPost("upload/{dishId}")]
        public async Task<IActionResult> UploadImage(int dishId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Генерируем уникальное имя для файла
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_imageFolderPath, fileName);

            // Сохраняем файл на сервере
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Получаем URL изображения
            var imageUrl = $"{Request.Scheme}://{Request.Host}/images/{fileName}";

            // Обновляем запись о блюде в базе данных, добавляем путь к изображению
            var dish = await _context.Dishes.FindAsync(dishId);
            if (dish == null)
            {
                return NotFound("Dish not found.");
            }

            // Обновляем поле с URL изображения
            dish.ImageUrl = imageUrl;
            await _context.SaveChangesAsync();

            return Ok(new { imageUrl = imageUrl });
        }
    }
}
