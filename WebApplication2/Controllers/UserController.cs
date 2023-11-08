using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController : BaseController
    {
        public async Task<IActionResult> ProfileInfo()
        {

            string token = GetToken();
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
                        return await ProfileInfo();
                    }
                    else if (data.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (data.ErrorCode == 0)
                    {
                        return View("~/Views/User/ProfileInfo.cshtml", data.User);
                    }



                }
            }
            return View("~/Views/Account/Login.cshtml"); /////////!!!!!!
        }
    }
}
