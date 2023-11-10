using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Models.API;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        //public async Task<IActionResult> Index()
        //{
        //    string token = GetToken();//Work

        //    using (var httpClient = new HttpClient())
        //    {

        //        var apiUrl = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

        //        var response = await httpClient.GetAsync(apiUrl);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Чтение данных из HTTP-ответа.

        //            var data = await response.Content.ReadAsAsync<GetProfileInfo>();
        //            if (data.ErrorCode == 143)
        //            {
        //                await RefreshToken();
        //                return await Index();
        //            }
        //            else if (data.ErrorCode == 118)
        //            {
        //                return View("~/Views/Account/Login.cshtml");
        //            }
        //            else if (data.ErrorCode == 0)
        //            {
        //                return View("~/Views/Home/Index.cshtml", data.User);
        //            }



        //        }
        //    }


        //    return View("~/Views/Home/Index.cshtml");
        //}

        public async Task<IActionResult> Index()
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
                        return await Index();
                    }
                    else if (data.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (data.ErrorCode == 0)
                    {
                        using (var httpClient1 = new HttpClient())
                        {

                            var apiUrl1 = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaires?token=" + token;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var response1 = await httpClient1.GetAsync(apiUrl1);

                            if (response1.IsSuccessStatusCode)
                            {
                                // Чтение данных из HTTP-ответа.

                                var data1 = await response1.Content.ReadAsAsync<GetQuestionnaireInfo>();
                                if (data1.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Index();
                                }
                                else if (data1.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (data1.errorCode == 0)
                                {
                                    return View("~/Views/Home/Index.cshtml", data1.questionnaires);
                                }



                            }
                        }
                    }



                }
            }




            return View("~/Views/Home/Index.cshtml");
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionnaireViewModel VM)
        {
            return View();

        }

        [HttpGet]   //!
        public async Task<IActionResult> AddQuestion()
        {
            return PartialView("~/Views/Home/_AddQuestion.cshtml");
        }

    }
}
