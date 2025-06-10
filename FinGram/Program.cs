using FinGram.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=FinGram;Username=postgres;Password=PassWord11"));
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.TestConnection())
    {
        Console.WriteLine("Подключение к базе данных успешно установлено.");
    }
    else
    {
        Console.WriteLine("Не удалось подключиться к базе данных.");
    }
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();