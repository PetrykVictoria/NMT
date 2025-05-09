using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMT.Models;
using System.Net.Http.Headers;

namespace NMT.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            // Надсилаємо запит до API
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:7029/api/auth/register", model);


            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
                // TODO: Зберегти токен у куки або сесію
                return RedirectToAction("Index", "Topics");
            }

            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:7029/api/auth/register", model);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                // ⬇️ Зберігаємо JWT-токен у cookie
                Response.Cookies.Append("jwt_token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(3)
                });

                // ⬇️ Переходимо на захищену сторінку
                return RedirectToAction("Index", "Topics");
            }

            ModelState.AddModelError("", "Помилка під час реєстрації");
            return View(model);
        }
    //    [Authorize]
    //    public async Task<IActionResult> Profile()
    //    {
    //        // Зчитуємо токен із кукі
    //        var token = Request.Cookies["jwt_token"];
    //        if (string.IsNullOrEmpty(token))
    //            return RedirectToAction("Login");

    //        // Викликаємо API, щоб витягти дані поточного користувача
    //        using var client = new HttpClient();
    //        client.DefaultRequestHeaders.Authorization
    //            = new AuthenticationHeaderValue("Bearer", token);

    //        var apiResponse = await client.GetFromJsonAsync<UserProfileVm>(
    //            "https://localhost:7029/api/user/profile");
    //        if (apiResponse == null)
    //            return View("Error");

    //        return View(apiResponse);
    //    }

    //    [Authorize]
    //    public IActionResult EditProfile()
    //    {
    //        // Приклад: просто віддаємо пустий VM, або можна попередньо з API заповнити
    //        var vm = new EditProfileVm { /* ... */ };
    //        return View(vm);
    //    }

    //    [Authorize]
    //    [HttpPost]
    //    public async Task<IActionResult> EditProfile(EditProfileVm vm)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(vm);

    //        var token = Request.Cookies["jwt_token"];
    //        using var client = new HttpClient();
    //        client.DefaultRequestHeaders.Authorization
    //            = new AuthenticationHeaderValue("Bearer", token);

    //        var response = await client.PutAsJsonAsync(
    //            "https://localhost:7029/api/user/profile", vm);
    //        if (!response.IsSuccessStatusCode)
    //        {
    //            ModelState.AddModelError("", "Не вдалося оновити профіль");
    //            return View(vm);
    //        }

    //        return RedirectToAction(nameof(Profile));
    //    }
    }
}


