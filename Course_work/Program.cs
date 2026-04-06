using Microsoft.EntityFrameworkCore;
using Course_Work.Models;
using Course_Work.Services;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// EPPlus License для версии 8.5.1
// ========================================
ExcelPackage.License.SetNonCommercialPersonal("Your Name");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<EsportsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ExcelService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseDefaultFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/export", async ([Microsoft.AspNetCore.Mvc.FromServices] ExcelService service) =>
{
    try
    {
        var fileBytes = await service.GetExcelAsync();
        var fileName = $"Esports_Data_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        
        return Results.File(
            fileBytes, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            fileName
        );
    }
    catch (Exception ex)
    {
        return Results.Problem($"Ошибка при генерации Excel файла: {ex.Message}");
    }
});

app.Run();