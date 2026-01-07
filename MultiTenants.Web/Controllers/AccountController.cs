using Microsoft.AspNetCore.Mvc;
using MultiTenants.Web.Interfaces;
using MultiTenants.Web.Models;
using MultiTenants.Web.Services;
using System.Net.Http;
using System.Reflection;

namespace MultiTenants.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiClient _apiClient;

        public AccountController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Create HttpRequestMessage for login
            var request = new HttpRequestMessage(HttpMethod.Post, "account/login")
            {
                Content = JsonContent.Create(model)
            };

            // Send request via generic ApiClient
            var response = await _apiClient.SendAsync<TokenResponseModel>(request);

            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            // Store access token (memory)
            HttpContext.Session.SetString("access_token", response.AccessToken);
            HttpContext.Session.SetInt32("expires_in", response.ExpiresIn);

            // Store refresh token in HttpOnly cookie
            Response.Cookies.Append(
                "refresh_token",
                response.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            // Create HttpRequestMessage for login
            var request = new HttpRequestMessage(HttpMethod.Get, "Home");

            // Send request via generic ApiClient
            var response = await _apiClient.SendAsync<string>(request);

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
