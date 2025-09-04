using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;


namespace NZWalks.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }



        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            // Send model → API (property names match DTO)
            var response = await client.PostAsJsonAsync("https://localhost:7050/api/auth/register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "User registered successfully! Please login.";
                return RedirectToAction("Login");
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Registration failed: {errorMsg}");
            return View(model);
        }




        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7050/api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                // ✅ Store JWT in cookie
                Response.Cookies.Append("JWTToken", tokenResponse.Token, new CookieOptions
                {
                    HttpOnly = true,   // prevents JS access
                    Secure = false,     // true --> to hide use only over HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1) // set expiry
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTToken"); // clear JWT cookie
            return RedirectToAction("Login");
        }

    }
}
