using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ISQuiz.Models;
using ISQuiz.Models.Enum;

namespace ISQuiz.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

        public string GetToken()
        {
            try
            {
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
                        var name = nameClaim.Value; // Получите значение Claim
                        return name;
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //throw;
            }
            return null;
        }


        public async Task RefreshToken()
        {
            try
            {
                //Getting principal claims that are read-only
                var claimPrincipal = User as ClaimsPrincipal;
                //Getting claimIdentity from principalClaim
                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                //Finding claim we need to read or edit
                var claim = (from c in claimPrincipal.Claims
                             where c.Type == ".AspNetCore.Admin"
                             select c).FirstOrDefault();

                using (var httpClientForRefreshToken = new HttpClient())
                {
                    // Замените URL на ссылку, с которой вы хотите получить данные.
                    var apiUrlForRefreshToken = "https://dev.edi.md/ISAuthService/json/RefreshToken?Token=" + claim.Value;

                    // Преобразуйте объект loginVM в JSON и отправьте его с помощью HttpContent.
                    // var jsonContent = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");

                    var responseForRefreshToken = await httpClientForRefreshToken.GetAsync(apiUrlForRefreshToken);

                    if (responseForRefreshToken.IsSuccessStatusCode)
                    {
                        // Чтение данных из HTTP-ответа.
                        var userData = await responseForRefreshToken.Content.ReadAsAsync<GetProfileInfo>();
                        if (!string.IsNullOrEmpty(userData.Token))
                        {

                            //SignOut to unlock claims for mananging them
                            HttpContext.SignOutAsync();

                            //Creating new claim to change our token after refresh
                            var claimNew = new Claim(".AspNetCore.Admin", userData.Token);
                            //Try remove claim selected from claimIdentity
                            //If we use RemoveClaim we can get excetion if claim can't be removed
                            claimIdentity.TryRemoveClaim(claim);
                            claimIdentity.AddClaim(claimNew);
                            //Add new claim
                            //SignIn to save claim principal
                            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);


                        }

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
                var uiLanguage = EnUiLanguage.RU;
                List<string> cultures = new List<string>() { "en", "ro", "ru" };
                if (!cultures.Contains(shortLang))
                {
                    shortLang = "ru";
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
                        uiLanguage = EnUiLanguage.RU;
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


        //serice query for change language
        [HttpGet]
        public async Task<IActionResult> ChangeLanguage(int lang, string returnUrl)
        {
            try
            {
                var token = GetToken();
                using (var httpClientForChangeLanguage = new HttpClient())
                {
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


            }
            catch (Exception ex)
            {
                /*_logger.Error(ex, ex.Message);
                return CreateJsonKo(Localization.NotSaved, true);*/
                return null;
            }
            return null;
        }

        public string GetLanguageCookie()
        {
            var cookie = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            if (cookie == null)
            {
                return "ru";
            }
            else
            {
                List<string> cultures = new List<string>() { "en", "ro", "ru" };
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


        /*
                public string GetLanguageCookie()
                {
                    var cookie = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
                    if (cookie == null)
                    {
                        return "ru";
                    }
                    else
                    {
                        List<string> cultures = new List<string>() { "en", "ro", "ru" };
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

                public async Task<string> ChangeLanguageCookie(string language)
                {
                    EnUiLanguage uiLanguage = new EnUiLanguage();//
                    switch (language)
                    {
                        case "en":
                            uiLanguage = EnUiLanguage.EN; break;
                        case "ru":
                            uiLanguage = EnUiLanguage.RU; break;
                        case "ro":
                            uiLanguage = EnUiLanguage.RO; break;
                        default:
                            break;
                    }

                    string token = GetToken();



                    using (var httpClientForChangePassword = new HttpClient())
                    {
                        var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISAuthService/json/ChangeUILanguage?Token=" + token + "&Language=" + uiLanguage;

                        var responseGetQuestionnaires = await httpClientForChangePassword.GetAsync(apiUrlGetQuestionnairesByToken);
                        if (responseGetQuestionnaires.IsSuccessStatusCode)
                        {
                            var baseResponseData = await responseGetQuestionnaires.Content.ReadAsAsync<BaseResponse>();
                            if (baseResponseData.ErrorCode == 143)
                            {
                                await RefreshToken();
                                return await ChangeLanguageCookie(language);
                            }
                            else if (baseResponseData.ErrorCode == 118)
                            {

                            }
                            else if (baseResponseData.ErrorCode == 0)
                            {
                                var claimPrincipal = User as ClaimsPrincipal;
                                //Getting claimIdentity from principalClaim
                                var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
                                //Finding claim we need to read or edit
                                var claim = (from c in claimPrincipal.Claims
                                             where c.Type == "UiLanguage"
                                             select c).FirstOrDefault();

                                //SignOut to unlock claims for mananging them
                                HttpContext.SignOutAsync();

                                //Creating new claim to change our token after refresh
                                var claimNew = new Claim("UiLanguage", language);
                                //Try remove claim selected from claimIdentity
                                //If we use RemoveClaim we can get excetion if claim can't be removed
                                claimIdentity.TryRemoveClaim(claim);
                                claimIdentity.AddClaim(claimNew);
                                //Add new claim
                                //SignIn to save claim principal
                                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);
                                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language.ToString())), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
                            }

                        }


                    }


                    return null;

                }*/


    }
}
