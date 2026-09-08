using Microsoft.AspNetCore.Mvc;
using Session.Models;

namespace Session.Controllers
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
                HttpContext.Session.SetString("login", login.UserName!); // створення сесійної змінної (доречі, саму сесію в асп не треба створювати)
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
    }
}
