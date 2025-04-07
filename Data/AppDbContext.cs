using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Dto;

namespace MyRestoranApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet <Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Category> Categories { get; set; } 
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<CategoryWithDishesDto> CategoryWithDishesDto { get; set; }
    }
}
