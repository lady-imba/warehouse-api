using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{

    var envConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    
    var configConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    var connectionString = !string.IsNullOrEmpty(envConnectionString) 
        ? envConnectionString 
        : (configConnectionString ?? "Data Source=warehouse.db");
    
    Console.WriteLine($"Используется строка подключения: {connectionString}");
    
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IRollService, RollService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
    }
}

app.Run();