using Microsoft.AspNetCore.Mvc;
using Cookie.Models;

namespace Cookie.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Login login)
        {
            if (ModelState.IsValid)
            {
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10); // термін збереження кукі - 10 днів
                Response.Cookies.Append("login", login.UserName, option); // створення кукі
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
    }
}