using ISQuiz.Models;
using ISQuiz.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISQuiz.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private readonly ILogger<BaseController> _logger;
        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }
        public string GetToken()
        {
            try
            {
                _logger.LogInformation("GetToken BaseController");
                var user = User;
                // Проверьте, аутентифицирован ли пользователь
                if (user.Identity.IsAuthenticated)
                {
                    // Получите все Claim-ы пользователя
                    var claims = user.Claims;

                    // Получите конкретный Claim по типу
                    var nameClaim = user.FindFirst(".AspNetCore.Admin");    //token

                    if (nameClaim != null)
                    {
                        return nameClaim.Value; // Получите значение Claim
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }


        public async Task RefreshToken()
        {
            try
            {
                _logger.LogInformation("RefreshToken BaseController");
                //Getting principal claims that are read-only
                var claimPrincipal = User as ClaimsPrincipal;
                //Getting claimIdentity from principalClaim
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                //Finding claim we need to read or edit
                var claim = (from c in claimPrincipal.Claims
                             where c.Type == ".AspNetCore.Admin"
                             select c).FirstOrDefault();

                using var httpClientForRefreshToken = new HttpClient();

                var apiUrlForRefreshToken = "https://dev.edi.md/ISAuthService/json/RefreshToken?Token=" + claim.Value;

                var responseForRefreshToken = await httpClientForRefreshToken.GetAsync(apiUrlForRefreshToken);

                if (responseForRefreshToken.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.
                    var userData = await responseForRefreshToken.Content.ReadAsAsync<GetProfileInfo>();
                    if (!string.IsNullOrEmpty(userData.Token))
                    {

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

                }
            }
            catch (Exception ex)
            {
                //throw;
            }


        }

        [HttpGet]
        public async Task<IActionResult> ChangeCulture(string shortLang, string returnUrl)
        {
            try
            {
                _logger.LogInformation("ChangeCulture BaseController");
                var uiLanguage = EnUiLanguage.EN;
                List<string> cultures = new() { "en", "ro", "ru" };
                if (!cultures.Contains(shortLang))
                {
                    shortLang = "en";
                }

                uiLanguage = shortLang switch
                {
                    "en" => EnUiLanguage.EN,
                    "ro" => EnUiLanguage.RO,
                    "ru" => EnUiLanguage.RU,
                    _ => EnUiLanguage.EN,
                };


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
                var uiLanguage = EnUiLanguage.EN;
                return await ChangeLanguage((int)uiLanguage, returnUrl);
            }
        }


        //serice query for change language
        [HttpGet]
        public async Task<IActionResult> ChangeLanguage(int lang, string returnUrl)
        {
            try
            {
                _logger.LogInformation("ChangeLanguage BaseController");
                var token = GetToken();
                using var httpClientForChangeLanguage = new HttpClient();
                var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISAuthService/json/ChangeUILanguage?Token=" + token + "&Language=" + lang;

                var responseGetQuestionnaires = await httpClientForChangeLanguage.GetAsync(apiUrlGetQuestionnairesByToken);
                if (responseGetQuestionnaires.IsSuccessStatusCode)
                {
                    var baseResponseData = await responseGetQuestionnaires.Content.ReadAsAsync<BaseResponse>();
                    if (baseResponseData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await ChangeLanguage(lang, returnUrl);
                    }
                    else if (baseResponseData.ErrorCode == 118)
                    {

                    }
                    else if (baseResponseData.ErrorCode != 0)
                    {

                    }
                    return LocalRedirect(returnUrl ?? "/");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeLanguage catch\n" + ex.Message);
                return null;
            }
            return null;
        }

        public string GetLanguageCookie()
        {
            _logger.LogInformation("GetLanguageCookie BaseController");
            var cookie = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (cookie == null)
            {
                return "en";
            }
            else
            {
                List<string> cultures = new() { "en", "ro", "ru" };
                if (cultures.Contains(cookie.ToLower()))
                {
                    return cookie.ToLower();
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
}
