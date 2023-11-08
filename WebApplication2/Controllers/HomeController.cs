using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            string token = GetToken();//Work

            using (var httpClient = new HttpClient())
            {

                var apiUrl = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var data = await response.Content.ReadAsAsync<GetProfileInfo>();
                    if (data.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Index();
                    }
                    else if (data.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (data.ErrorCode == 0)
                    {
                        return View("~/Views/Home/Index.cshtml", data.User);
                    }



                }
            }


            return View("~/Views/Home/Index.cshtml");
        }
    }
}
