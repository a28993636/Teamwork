
using Microsoft.EntityFrameworkCore;
using Teamwork.Filters;
using Teamwork.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TeamworkContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("TeamworkConnection")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddScoped<LoginStatusFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
