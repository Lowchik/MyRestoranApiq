using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление поддержки контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Маршрут для контроллеров API
app.MapControllers();

app.Run();
