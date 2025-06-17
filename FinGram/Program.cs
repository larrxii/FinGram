using FinGram.Data;
using FinGram.Models;
using FinGram.Services;
using FinGram.Pages.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;           
    options.Password.RequireLowercase = true;      
    options.Password.RequireUppercase = false;      
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 6;           
    options.Password.RequiredUniqueChars = 0;       
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddHostedService<TelegramBotService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapPost("/api/telegramauth", async (AppDbContext db, TelegramAuthModel.TelegramAuthRequest request) =>
{
    var link = await db.TelegramLinks
        .Include(x => x.User)
        .FirstOrDefaultAsync(x => x.Token == request.Token /* && !x.IsUsed */);

    if (link == null)
        return Results.NotFound(new { error = "Invalid or used token." });

    link.TelegramId = request.TelegramId;
    link.IsUsed = true;
    await db.SaveChangesAsync();

    return Results.Json(new
    {
        userId = link.UserId,
        userName = link.User.UserName,
        email = link.User.Email
    });
});


app.Run();