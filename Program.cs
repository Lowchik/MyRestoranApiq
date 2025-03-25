using Microsoft.EntityFrameworkCore;
using MyRestoranApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ���������� ��������� ���� ������
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� ��������� ������������
builder.Services.AddControllers();

// ���������� CORS (��������� ��� ������, ������ � ���������)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()    // ��������� ��� ���������
               .AllowAnyMethod()    // ��������� ����� ������ (GET, POST � �.�.)
               .AllowAnyHeader();   // ��������� ����� ���������
    });
});

// ���������� Swagger ��� API ������������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ��������� Swagger UI � ������ ����������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ��������� ��������� �� HTTPS
app.UseHttpsRedirection();

// ��������� CORS
app.UseCors("AllowAllOrigins");  // ��� ����� ��� ���������� CORS

// ������� ��� ������������ API
app.MapControllers();

app.Run();
