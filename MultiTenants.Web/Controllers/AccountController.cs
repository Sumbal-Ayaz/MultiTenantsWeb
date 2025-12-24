using Microsoft.AspNetCore.Mvc;
using MultiTenants.Web.Models;

namespace MultiTenants.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Email == "user@test.com" && model.Password == "user123")
            {
                return RedirectToAction("Index", "User");
            }
            else if (model.Email == "admin@test.com" && model.Password == "admin123")
            {
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError("", "Invalid UserName or Email");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            return RedirectToAction("Login");
        }
    }
}
