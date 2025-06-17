using FinGram.Data;
using FinGram.Models;
using FinGram.Services;
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
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddScoped<TestService>();

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

app.Run();