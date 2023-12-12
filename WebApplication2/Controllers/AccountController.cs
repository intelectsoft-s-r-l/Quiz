using WebApplication2.Filter;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Interface;
using WebApplication2.Models.Enum;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Culture]
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

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
                return View("Login", loginVM);
            try
            {
                var userData = await _accountRepository.AuthorizeUser(loginVM);

                if (userData.ErrorCode == 0)
                {
                    List<Claim> userClaims = new List<Claim>();


                    switch (GetLanguageCookie())
                    {
                        case "en":
                            userData.User.UiLanguage = EnUiLanguage.EN; break;
                        case "ro":
                            userData.User.UiLanguage = EnUiLanguage.RO; break;
                        case "ru":
                            userData.User.UiLanguage = EnUiLanguage.RU; break;
                        default:
                            userData.User.UiLanguage = EnUiLanguage.RU; break;
                    }


                    var baseResponseData = await _accountRepository.ChangeUILanguage(userData.Token, userData.User.UiLanguage);
                    if (baseResponseData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Login(loginVM);
                    }
                    else if (baseResponseData.ErrorCode != 0)
                    {
                        return View("Error");
                    }


                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userData.User.ID.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Email, userData.User.Email));
                    userClaims.Add(new Claim("FullName", userData.User.FirstName + " " + userData.User.LastName));
                    userClaims.Add(new Claim("Company", userData.User.Company));
                    userClaims.Add(new Claim("PhoneNumber", userData.User.PhoneNumber));
                    userClaims.Add(new Claim(".AspNetCore.Admin", userData.Token));
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
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
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

            var baseResponseData = await _accountRepository.RecoverPassword(recoverpwVM);

            if (baseResponseData.ErrorCode == 0)
            {
                AuthorizeViewModel authorizeViewModel = new AuthorizeViewModel();
                authorizeViewModel.Email = recoverpwVM.Email;
                TempData["Success"] = Localization.successRecoverPWMessage;
                return View("~/Views/Account/Login.cshtml", authorizeViewModel);
            }
            else
            {
                TempData["Error"] = baseResponseData.ErrorMessage ?? "Undefined";
                return View("~/Views/Account/AuthRecoverpw.cshtml");
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

