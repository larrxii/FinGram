using FinGram.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=FinGram;Username=your_username;Password=your_password"));
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.TestConnection())
    {
        Console.WriteLine("����������� � ���� ������ ������� �����������.");
    }
    else
    {
        Console.WriteLine("�� ������� ������������ � ���� ������.");
    }
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();