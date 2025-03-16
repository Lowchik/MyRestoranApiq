using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление поддержки контроллеров
builder.Services.AddControllers();

// Добавление Swagger для API тестирования
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включение Swagger UI в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включение редиректа на HTTPS
app.UseHttpsRedirection();

// Маршрут для контроллеров API
app.MapControllers();

app.Run();
