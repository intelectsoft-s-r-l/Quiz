using ISQuiz.Filter;
using ISQuiz.Interface;
using ISQuiz.Models.Enum;
using ISQuiz.Resources;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISQuiz.Controllers
{
    [Culture]
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountRepository accountRepository,
                                 ILogger<AccountController> logger) : base(logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View("~/Views/Account/Login.cshtml");


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthorizeViewModel loginVM)
        {
            _logger.LogInformation($"Login method called.");

            if (!ModelState.IsValid)
                return View("Login", loginVM);

            try
            {
                var userData = await _accountRepository.AuthorizeUser(loginVM);

                if (userData.ErrorCode == 0)
                {
                    userData.User.UiLanguage = GetLanguageCookie() switch
                    {
                        "en" => EnUiLanguage.EN,
                        "ro" => EnUiLanguage.RO,
                        "ru" => EnUiLanguage.RU,
                        _ => EnUiLanguage.EN,
                    };
                    await HandleLanguageChange(userData.Token, userData.User.UiLanguage);

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userData.User.ID.ToString()),
                        new Claim(ClaimTypes.Email, userData.User.Email),
                        new Claim("FullName", userData.User.FirstName + " " + userData.User.LastName),
                        new Claim("Company", userData.User.Company),
                        new Claim("PhoneNumber", userData.User.PhoneNumber),
                        new Claim(".AspNetCore.Admin", userData.Token),
                        new Claim("UiLanguage", GetLanguageCookie())
                    };

                    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(GetLanguageCookie().ToString())),
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    TempData["Error"] = userData.ErrorMessage ?? "Undefined";
                    return RedirectToAction(nameof(Login), new { error = userData.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Login method. " + ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }

        private async Task HandleLanguageChange(string token, EnUiLanguage uiLanguage)
        {
            var baseResponseData = await _accountRepository.ChangeUILanguage(token, uiLanguage);
            if (baseResponseData.ErrorCode == 143)
            {
                await RefreshToken();
                await HandleLanguageChange(token, uiLanguage); // рекурсивный вызов
            }
            else if (baseResponseData.ErrorCode != 0)
            {
                _logger.LogError($"{baseResponseData}");
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
            _logger.LogInformation($"AuthRecoverpw method called.");
            if (!ModelState.IsValid)
                return View();
            try
            {
                var baseResponseData = await _accountRepository.RecoverPassword(recoverpwVM);

                if (baseResponseData.ErrorCode == 0)
                {
                    AuthorizeViewModel authorizeViewModel = new()
                    {
                        Email = recoverpwVM.Email
                    };
                    TempData["Success"] = Localization.successRecoverPWMessage;
                    return View("~/Views/Account/Login.cshtml", authorizeViewModel);
                }
                else
                {
                    TempData["Error"] = baseResponseData.ErrorMessage ?? "Undefined";
                    return View("~/Views/Account/AuthRecoverpw.cshtml");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the AuthRecoverpw method." + ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml");
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
            _logger.LogInformation($"ChangeCultureLogin method called.");
            try
            {
                List<string> cultures = new() { "en", "ro", "ru" };
                if (!cultures.Contains(shortLang))
                {
                    shortLang = "en";
                }

                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(shortLang)),
                                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the ChangeCultureLogin method." + ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml");
            }


        }


    }


}

