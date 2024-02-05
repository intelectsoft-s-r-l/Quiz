using ISQuiz.Interface;
using ISQuiz.Models.Enum;
using ISQuiz.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace ISQuiz.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private readonly IAccountRepository _accountRepository = new AccountRepository();
        public static string refreshedToken = "";
        public static object __refreshTokenLock = new();
        //private IViewRenderService _viewRenderService;
        //protected IViewRenderService ViewRenderService => _viewRenderService ??= HttpContext.RequestServices.GetService<IViewRenderService>();


        public async Task<bool> RefreshToken()
        {
            bool retObject = true;

            try
            {
                //Log.Information("Try RefreshToken");
                //Monitor.Enter(__refreshTokenLock);
                //Getting principal claims that are read-only
                var claimPrincipal = User as ClaimsPrincipal;
                //Getting claimIdentity from principalClaim
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                //Finding claim we need to read or edit
                var claim = (from c in claimPrincipal.Claims
                             where c.Type == ".AspNetCore.Admin"
                             select c).FirstOrDefault();

                var userData = await _accountRepository.RefreshToken(Uri.EscapeDataString(claim.Value));
                if (!string.IsNullOrEmpty(userData.Token))
                {
                    refreshedToken = userData.Token;

                    //SignOut to unlock claims for mananging them
                    await HttpContext.SignOutAsync();

                    //Creating new claim to change our token after refresh
                    var claimNew = new Claim(".AspNetCore.Admin", userData.Token);
                    //Try remove claim selected from claimIdentity
                    //If we use RemoveClaim we can get excetion if claim can't be removed
                    claimIdentity.TryRemoveClaim(claim);
                    claimIdentity.AddClaim(claimNew);
                    //Add new claim
                    //SignIn to save claim principal
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
                }
                else
                {
                    Log.Information("Refresh method. Response => {@userData}", userData);
                    retObject = false;
                }
                if (userData.ErrorCode == EnErrorCode.Incorrect_refresh_token)
                {
                    retObject = false;
                }
                return retObject;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                retObject = false;
                return retObject;
            }
            //finally { Monitor.Exit(__refreshTokenLock); }
        }

        //Monitor.Enter(
        public string GetToken()
        {
            try
            {
                //Log.Information("GetToken");
                Monitor.Enter(__refreshTokenLock);
                //Getting principal claims that are read-only
                var claimPrincipal = User as ClaimsPrincipal;
                //Getting claimIdentity from principalClaim
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                //Finding claim we need to read or edit
                var claim = (from c in claimPrincipal.Claims
                             where c.Type == ".AspNetCore.Admin"
                             select c).FirstOrDefault();
                //returning claimValue that is our token for service requests
                return Uri.EscapeDataString(claim.Value.ToString());
            }
            finally { Monitor.Exit(__refreshTokenLock); }
        }


        [HttpGet]
        public async Task<IActionResult> ChangeCulture(string shortLang, string returnUrl)
        {
            try
            {
                //Log.Information($"ChangeCulture | {shortLang}");
                var uiLanguage = EnUiLanguage.RU;
                List<string> cultures = new List<string>() { "en", "ro", "ru" };
                if (!cultures.Contains(shortLang))
                {
                    shortLang = "en";
                }

                switch (shortLang)
                {
                    case "en":
                        uiLanguage = EnUiLanguage.EN;
                        break;
                    case "ro":
                        uiLanguage = EnUiLanguage.RO;
                        break;
                    case "ru":
                        uiLanguage = EnUiLanguage.RU;
                        break;
                    default:
                        uiLanguage = EnUiLanguage.EN;
                        break;
                }


                //var userClaims = GetUserClaims();

                //Getting principal claims that are read-only
                var claimPrincipal = User as ClaimsPrincipal;
                //Getting claimIdentity from principalClaim
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                //Finding claim we need to read or edit
                var claim = (from c in claimPrincipal.Claims
                             where c.Type == "UiLanguage"
                             select c).Single();

                if (claim != null)
                {
                    //SignOut to unlock claims for mananging them
                    await HttpContext.SignOutAsync();

                    //Creating new claim to change our token after refresh
                    var claimNew = new Claim("UiLanguage", uiLanguage.ToString());
                    //Try remove claim selected from claimIdentity
                    //If we use RemoveClaim we can get excetion if claim can't be removed
                    claimIdentity.TryRemoveClaim(claim);
                    //Add new claim
                    claimIdentity.AddClaim(claimNew);
                    //SignIn to save claim principal
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
                }
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(shortLang)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                return await ChangeLanguage((int)uiLanguage, returnUrl);
            }
            catch (Exception ex)
            {
                var uiLanguage = EnUiLanguage.RU;
                return await ChangeLanguage((int)uiLanguage, returnUrl);
            }
        }


        [HttpGet]
        public async Task<IActionResult> ChangeLanguage(int lang, string returnUrl)
        {
            try
            {
                //Log.Information("ChangeLanguage");
                var token = GetToken();

                var response = await _accountRepository.ChangeUILanguage(token, (EnUiLanguage)lang);

                if (response.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken())
                    {
                        return await ChangeLanguage(lang, returnUrl);
                    }
                }
                return LocalRedirect(returnUrl ?? "/");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml", ex);
            }
        }

        public string GetLanguageCookie()
        {
            var cookie = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (cookie == null)
            {
                return "en";
            }
            else
            {
                var c_uic = cookie.Split('|');
                var culture = c_uic[0].Split("=");

                return culture[1];
            }
        }

    }
}
