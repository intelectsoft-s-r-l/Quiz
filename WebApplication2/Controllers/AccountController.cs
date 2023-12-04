using ISAdminWeb.Filter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Models.Enum;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Culture]
    public class AccountController : BaseController
    {


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            string languageFromCookie = GetLanguageCookie();

            if (string.IsNullOrEmpty(languageFromCookie))
            {
                ViewBag.Language = "ru";
            }
            else
                ViewBag.Language = languageFromCookie;
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthorizeViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }
            else
            {
                using (var httpClientForAuth = new HttpClient())
                {
                    var apiUrlForAuth = "https://dev.edi.md/ISAuthService/json/AuthorizeUser";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");

                    var responseAuth = await httpClientForAuth.PostAsync(apiUrlForAuth, jsonContent);

                    if (responseAuth.IsSuccessStatusCode)
                    {
                        // Чтение данных из HTTP-ответа.
                        var userData = await responseAuth.Content.ReadAsAsync<GetProfileInfo>();

                        if (userData.ErrorCode == 0)
                        {
                            List<Claim> userClaims = new List<Claim>();
                            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userData.User.ID.ToString()));
                            userClaims.Add(new Claim(ClaimTypes.Email, userData.User.Email));
                            userClaims.Add(new Claim("FullName", userData.User.FirstName + " " + userData.User.LastName));
                            userClaims.Add(new Claim("Company", userData.User.Company));
                            userClaims.Add(new Claim("PhoneNumber", userData.User.PhoneNumber));
                            userClaims.Add(new Claim(".AspNetCore.Admin", userData.Token));

                            switch (GetLanguageCookie())
                            {
                                case "en":
                                    userData.User.UiLanguage = EnUiLanguage.EN; break;
                                case "ro":
                                    userData.User.UiLanguage = EnUiLanguage.RO; break;
                                case "ru":
                                    userData.User.UiLanguage = EnUiLanguage.RU; break;
                                default:
                                    break;
                            }

                            using (var httpClientForChangeLanguage = new HttpClient())
                            {
                                var apiUrlUserLanguage = "https://dev.edi.md/ISAuthService/json/ChangeUILanguage?Token=" + userData.Token + "&Language=" + userData.User.UiLanguage;

                                var responseUserLanguage = await httpClientForChangeLanguage.GetAsync(apiUrlUserLanguage);
                                if (responseUserLanguage.IsSuccessStatusCode)
                                {
                                    var baseResponseData = await responseUserLanguage.Content.ReadAsAsync<BaseResponse>();
                                    if (baseResponseData.ErrorCode == 143)
                                    {
                                        await RefreshToken();
                                        return await Login(loginVM);
                                    }
                                    else if (baseResponseData.ErrorCode == 118)
                                    {

                                    }
                                    else if (baseResponseData.ErrorCode != 0)
                                    {

                                    }
                                }
                            }
                            /*
                            string userLanguage = "";
                            switch (userData.User.UiLanguage)
                            {
                                case EnUiLanguage.EN:
                                    userLanguage = "en";
                                    break;
                                case EnUiLanguage.RO:
                                    userLanguage = "ro";
                                    break;
                                case EnUiLanguage.RU:
                                    userLanguage = "ru";
                                    break;
                                default:
                                    break;
                            }*/

                            userClaims.Add(new Claim("UiLanguage", GetLanguageCookie()));
                            
                            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(GetLanguageCookie().ToString())), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
                            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                            var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                        else
                        {
                            TempData["Error"] = userData.ErrorMessage;
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

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AuthRecoverpw()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthRecoverpw(AuthRecoverpwViewModel recoverpwVM)
        {
            if (!ModelState.IsValid)
                return View();

            using (var httpClientForResetPW = new HttpClient())
            {

                var apiUrlForResetPW = "https://dev.edi.md/ISAuthService/json/ResetPassword";

                var jsonContent = new StringContent(JsonConvert.SerializeObject(recoverpwVM), Encoding.UTF8, "application/json");

                var response = await httpClientForResetPW.PostAsync(apiUrlForResetPW, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.
                    var baseResponseData = await response.Content.ReadAsAsync<BaseResponse>();

                    if (baseResponseData.ErrorCode == 0)
                    {
                        AuthorizeViewModel authorizeViewModel = new AuthorizeViewModel();
                        authorizeViewModel.Email = recoverpwVM.Email;
                        TempData["Success"] = Localization.SuccessPW;//"Check your email adress!";
                        return View("~/Views/Account/Login.cshtml", authorizeViewModel);
                    }
                    else
                    {
                        TempData["Error"] = baseResponseData.ErrorMessage;
                        return View("~/Views/Account/AuthRecoverpw.cshtml");
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
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangeCultureLogin(string shortLang)
        {
            List<string> cultures = new List<string>() { "en", "ro", "ru" };
            if (!cultures.Contains(shortLang))
            {
                shortLang = "ru";
            }

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(shortLang)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return RedirectToAction("Login");

        }


    }


}

