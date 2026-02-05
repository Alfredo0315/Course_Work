using Microsoft.EntityFrameworkCore;
using Course_Work.Models;

var builder = WebApplication.CreateBuilder(args);
// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:63342",  // WebStorm
                "http://127.0.0.1:63342",  // WebStorm альтернативный
                "http://localhost:5500",   // Live Server
                "http://127.0.0.1:5500"    // Live Server альтернативный
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Добавляем DbContext с подключением к SQL Server
builder.Services.AddDbContext<EsportsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Для PostgreSQL используйте:
// builder.Services.AddDbContext<EsportsDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем CORS если нужно
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
// Добавляем статические файлы
builder.Services.AddDirectoryBrowser();
var app = builder.Build();
app.UseCors("AllowAll");
// Используем статические файлы
app.UseStaticFiles();
app.UseDefaultFiles();
// Используем CORS
app.UseCors("AllowFrontend");
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();