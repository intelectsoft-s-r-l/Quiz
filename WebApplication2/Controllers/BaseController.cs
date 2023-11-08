using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;
using System.Linq;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

        public string GetToken()
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
            return null;
        }

        public async Task<bool> RefreshToken()
        {
            //Getting principal claims that are read-only
            var claimPrincipal = User as ClaimsPrincipal;
            //Getting claimIdentity from principalClaim
            var claimIdentity = claimPrincipal.Identity as ClaimsIdentity;
            //Finding claim we need to read or edit
            var claim = (from c in claimPrincipal.Claims
                         where c.Type == ".AspNetCore.Admin"
                         select c).FirstOrDefault();

            using (var httpClient = new HttpClient())
            {
                // Замените URL на ссылку, с которой вы хотите получить данные.
                var apiUrl = "https://dev.edi.md/ISAuthService/json/RefreshToken?Token=" + claim.Value;

                // Преобразуйте объект loginVM в JSON и отправьте его с помощью HttpContent.
                // var jsonContent = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.
                    var data = await response.Content.ReadAsAsync<GetProfileInfo>();
                    if (!string.IsNullOrEmpty(data.Token))
                    {

                        //SignOut to unlock claims for mananging them
                        HttpContext.SignOutAsync();

                        //Creating new claim to change our token after refresh
                        var claimNew = new Claim(".AspNetCore.Admin", data.Token);
                        //Try remove claim selected from claimIdentity
                        //If we use RemoveClaim we can get excetion if claim can't be removed
                        claimIdentity.TryRemoveClaim(claim);
                        claimIdentity.AddClaim(claimNew);
                        //Add new claim
                        //SignIn to save claim principal
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);


                    }

                }
                else
                {
                    // Обработка ошибки, если запрос не удался.
                    return false;
                }
            }
            return false;
        }

    }
}
