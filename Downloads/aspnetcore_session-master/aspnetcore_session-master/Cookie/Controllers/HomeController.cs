using Microsoft.AspNetCore.Mvc;

namespace Session.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // перевірка, чи користувач авторизований
            if (HttpContext.Session.GetString("login") == null)

            // if (!User.Identity.IsAuthenticated) - взагалі, для авторизації є окрема сучасна мідлварь, якщо налаштувати приблизно так:
            // в класі Program буде builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/Account/Login"; // куди редиректимо неавторизованих користувачів
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //});
            // builder.Services.AddAuthorization();
            // в ланцюжок мідлварєй додаємо
            // app.UseAuthentication(); // спочатку Authentication
            // app.UseAuthorization(); // потім Authorization, але цей приклад більш універсальний, в сесії можна зберігати багато чого :)
            {
                return RedirectToAction("Create", "Login");
            }
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // очищення сесії
            return RedirectToAction("Create", "Login");
        }
    }
}
