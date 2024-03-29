﻿using ISQuiz.Filter;
using ISQuiz.Interface;
using ISQuiz.Models.Enum;
using ISQuiz.Resources;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace ISQuiz.Controllers
{
    [Culture]
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() => View();
        

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthorizeViewModel loginVM)
        {
            //Log.Information("Into Account.Login | Post");

            if (!ModelState.IsValid)
                return View("Login", loginVM);

            try
            {
                var userData = await _accountRepository.AuthorizeUser(loginVM);

                if (userData.ErrorCode == EnErrorCode.NoError)
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
                else if(userData.ErrorCode == EnErrorCode.User_name_not_found_or_incorrect_password)
                {
                    TempData["Error"] = Localization.UserLoginNotFount;
                    return View("Login", loginVM);
                }
                else
                {
                    TempData["Error"] = userData.ErrorMessage;
                    Log.Information("Response => {@userData}", userData);
                    //return RedirectToAction(nameof(Login), new { error = userData.ErrorMessage });
                    //return View("~/Views/Account/Login.cshtml", loginVM);
                    return View("Login", loginVM);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }
        }

        private async Task HandleLanguageChange(string token, EnUiLanguage uiLanguage)
        {
            var baseResponseData = await _accountRepository.ChangeUILanguage(token, uiLanguage);
            if (baseResponseData.ErrorCode == EnErrorCode.Expired_token)
            {
                if (await RefreshToken())
                {
                    await HandleLanguageChange(token, uiLanguage);
                }
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult AuthRecoverpw() => View();



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthRecoverpw(AuthRecoverpwViewModel recoverpwVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                var baseResponseData = await _accountRepository.RecoverPassword(recoverpwVM);

                if (baseResponseData.ErrorCode == EnErrorCode.NoError)
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
                    TempData["Error"] = baseResponseData.ErrorMessage;
                    Log.Information("Response => {@baseResponseData}", baseResponseData);
                    return View("~/Views/Account/AuthRecoverpw.cshtml", recoverpwVM);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
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


    }


}

