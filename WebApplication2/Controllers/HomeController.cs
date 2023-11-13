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
using System.Net.Http;

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

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClient.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Index();
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClient1 = new HttpClient())
                        {

                            var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaires?token=" + token;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetQuestionnaires = await httpClient1.GetAsync(apiUrlGetQuestionnairesByToken);

                            if (responseGetQuestionnaires.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<GetQuestionnaireInfo>();
                                if (questionnaireData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Index();
                                }
                                else if (questionnaireData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (questionnaireData.errorCode == 0)
                                {
                                    return View("~/Views/Home/Index.cshtml", questionnaireData.questionnaires);
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
        public async Task<IActionResult> Create(CreateQuestionnaireViewModel CreateQuestionnaireVM)
        {
            
            string token = GetToken();
            using (var httpClient = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClient.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Create(CreateQuestionnaireVM);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClient1 = new HttpClient())
                        {
                            var apiUrlForPostQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/UpsertQuestionnaire";
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
                            //Correct model for post
                            CreateQuestionnaire createQuestionnaire = new CreateQuestionnaire();
                            createQuestionnaire.oid = 0;
                            createQuestionnaire.name = CreateQuestionnaireVM.Title;
                            List<CreateQuestion> tmp = new List<CreateQuestion>();
                            foreach(var question in CreateQuestionnaireVM.Questions)
                            {
                                CreateQuestion createQuestion = new CreateQuestion();
                                createQuestion.question = question.Text;
                                createQuestion.comentary = question.comentary;
                                createQuestion.gradingType = question.gradingType;
                                if(question.Answers.Any())
                                {
                                    createQuestion.answerVariants = new List<string>();
                                    foreach (var answer in question.Answers)
                                    {
                                        //if (answer.Text != null)
                                            createQuestion.answerVariants.Add(answer);
                                    }
                                }
                                tmp.Add(createQuestion);
                            }
                            createQuestionnaire.questions = tmp;
                            createQuestionnaire.companyOid = userData.User.CompanyID; //0
                            createQuestionnaire.token = token;
                            createQuestionnaire.company = userData.User.Company;

                            var jsonContent = new StringContent(JsonConvert.SerializeObject(createQuestionnaire), Encoding.UTF8, "application/json");

                            var responsePostQuestionnaire = await httpClient1.PostAsync(apiUrlForPostQuestionnaire, jsonContent);

                            if (responsePostQuestionnaire.IsSuccessStatusCode)
                            {
                                // Чтение данных из HTTP-ответа.

                                var questionnaireBaseResponsedData = await responsePostQuestionnaire.Content.ReadAsAsync<BaseErrors>(); //Тут была ошибка из-за названия переменных

                                if (questionnaireBaseResponsedData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Create(CreateQuestionnaireVM); ///!
                                }
                                else if (questionnaireBaseResponsedData.errorCode == 0)
                                {
                                    return View("~/Views/User/Settings.cshtml");
                                }



                            }
                        }
                    }



                }
            }

            return View("~/Views/Home/Index.cshtml");
        }


    }
}
