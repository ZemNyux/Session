var builder = WebApplication.CreateBuilder(args);

// всі сесії працюють поверх об'єкта IDistributedCache, і ASP.NET Core
// вже має вбудовану реалізацію IDistributedCache "в пам’яті"
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // тривалість сесії (тайм-аут бездіяльності)
    options.Cookie.Name = "Session"; // кожна сесія має свій ідентифікатор, який зберігається в cookie

});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// додаємо middleware для роботи з сесіями (обов'язково перед UseEndpoints / MapControllers)
app.UseSession();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();