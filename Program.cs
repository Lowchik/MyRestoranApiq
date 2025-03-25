using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление поддержки контроллеров
builder.Services.AddControllers();

// Добавление CORS (разрешаем все домены, методы и заголовки)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()    // Разрешаем все источники
               .AllowAnyMethod()    // Разрешаем любые методы (GET, POST и т.д.)
               .AllowAnyHeader();   // Разрешаем любые заголовки
    });
});

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

// Включение CORS
app.UseCors("AllowAllOrigins");  // Это важно для разрешения CORS

// Маршрут для контроллеров API
app.MapControllers();

app.Run();
