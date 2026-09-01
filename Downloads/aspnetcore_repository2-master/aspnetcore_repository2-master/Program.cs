using Microsoft.EntityFrameworkCore;
using mvc.Repository;

namespace mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<StudentContext>(options => options.UseSqlServer(connection));
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IEntityRepository<Student>, FirebaseStudentRepository>();

            var app = builder.Build();
            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Student}/{action=Index}/{id?}");

            app.Run();
        }
    }
}