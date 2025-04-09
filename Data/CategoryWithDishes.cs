namespace MyRestoranApi.Dto
{
    public class CategoryWithDishesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DishDto> Dishes { get; set; } = new();
    }

    public class DishDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
