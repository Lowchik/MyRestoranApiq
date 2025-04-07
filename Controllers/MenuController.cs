using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;
using MyRestoranApi.Dto;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly AppDbContext _context;

    public MenuController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("categories/dishes")]
    public async Task<IActionResult> GetCategoriesWithDishes()
    {
        try
        {
            var categories = await _context.Categories
                .Include(c => c.Dishes)
                .Select(c => new CategoryWithDishesDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Dishes = c.Dishes.Select(d => new DishDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Price = d.Price,
                        Description = d.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(categories);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении категорий с блюдами: {ex.Message}");
            return StatusCode(500, new { message = "Server error", error = ex.Message });
        }
    }


}
