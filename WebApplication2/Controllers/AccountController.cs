using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class AccountController : BaseController
    {

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            //var response = new AuthorizeViewModel();
            return View("~/Views/Account/Login.cshtml");//response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthorizeViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            using (var httpClient = new HttpClient())
            {
                // Замените URL на ссылку, с которой вы хотите получить данные.
                var apiUrl = "https://dev.edi.md/ISAuthService/json/AuthorizeUser";

                // Преобразуйте объект loginVM в JSON и отправьте его с помощью HttpContent.
                var jsonContent = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.
                    var data = await response.Content.ReadAsAsync<GetProfileInfo>();

                    if (data.ErrorCode == 0)
                    {
                        List<Claim> userClaims = new List<Claim>();
                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, data.User.ID.ToString()));
                        userClaims.Add(new Claim(ClaimTypes.Email, data.User.Email));
                        userClaims.Add(new Claim("FullName", data.User.FirstName + " " + data.User.LastName));
                        userClaims.Add(new Claim("Company", data.User.Company));
                        userClaims.Add(new Claim("PhoneNumber", data.User.PhoneNumber));
                        userClaims.Add(new Claim(".AspNetCore.Admin", data.Token));

                        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                        //Adding claimsIdentity to ClaimsPrincipal
                        var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                        //SignIn to save Claims in cookie, and re-use it in refresh token, after restarting application
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        TempData["Error"] = data.ErrorMessage;
                        return View(loginVM);
                    }
                }
                else
                {
                    // Обработка ошибки, если запрос не удался.
                    return View("Error");
                }
            }


        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            // return View("~/Views/Account/Login.cshtml");
            return RedirectToAction("Login", "Account");
        }
    }


}

