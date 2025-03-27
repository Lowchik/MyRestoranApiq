using Microsoft.EntityFrameworkCore;

namespace MyRestoranApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet <ReservationRequest> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

    }
}
