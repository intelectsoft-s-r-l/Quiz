using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication2.Interface;
using WebApplication2.Models;
using WebApplication2.Models.API;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileInfo()
        {
            string token = GetToken();
            var UserData = await _userRepository.getProfileInfo(token);
            if (UserData.ErrorCode == 0)
                return View("~/Views/User/ProfileInfo.cshtml", UserData.User);
            else if (UserData.ErrorCode == 143)
            {
                await RefreshToken();
                return await ProfileInfo();
            }
            else
                return View("~/Views/Account/Login.cshtml");


            //using (var httpClientForProfileInfo = new HttpClient())
            //{

            //    var apiUrlGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

            //    var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlGetProfileInfo);

            //    if (responseGetProfileInfo.IsSuccessStatusCode)
            //    {
            //        var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
            //        if (userData.ErrorCode == 143)
            //        {
            //            await RefreshToken();
            //            return await ProfileInfo();
            //        }
            //        else if (userData.ErrorCode == 118)
            //        {
            //            return View("~/Views/Account/Login.cshtml");
            //        }
            //        else if (userData.ErrorCode == 0)
            //        {
            //            return View("~/Views/User/ProfileInfo.cshtml", userData.User);
            //        }

            //    }
            //}
            //return View("Error");
            // return View("~/Views/Account/Login.cshtml"); /////////!!!!!!
        }


        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            string token = GetToken();
            var UserData = await _userRepository.getProfileInfo(token);
            if (UserData.ErrorCode == 0)
                return View("~/Views/User/Settings.cshtml", UserData.User);
            else if (UserData.ErrorCode == 143)
            {
                await RefreshToken();
                return await Settings();
            }
            else
                return View("~/Views/Account/Login.cshtml");
        }


        [HttpGet]   //!
        public async Task<IActionResult> ChangePassword()
        {
            return PartialView("~/Views/User/_ChangePassword.cshtml");
        }

        [HttpPost]  //NormalToken, mb problem
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)   //FromBody
        {

            if (!ModelState.IsValid)
                return Json(new { StatusCode = 500, Message = "No Valid" });

            string token = GetToken();

            ChangePasswordViewModel changePassword = new ChangePasswordViewModel()
            {
                NewPassword = changepwVM.NewPassword,
                OldPassword = changepwVM.OldPassword,
                Token = token
            };

            var baseResponseData = await _userRepository.changePassword(changePassword);
            if (baseResponseData.ErrorCode == 0)
                return Json(new { StatusCode = 200, Message = "Password changed successfully" });
            else if (baseResponseData.ErrorCode == 143)
            {
                await RefreshToken();
                return await ChangePassword(changepwVM);
            }
            else
                return Json(new { StatusCode = 500, Message = baseResponseData.ErrorMessage });

            //using (var httpClientForChangePW = new HttpClient())
            //{

            //    var apiUrlForChangePW = "https://dev.edi.md/ISAuthService/json/ChangePassword";

            //    var jsonContent = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");

            //    var responseChangePW = await httpClientForChangePW.PostAsync(apiUrlForChangePW, jsonContent);

            //    if (responseChangePW.IsSuccessStatusCode)
            //    {
            //        var baseResponseData = await responseChangePW.Content.ReadAsAsync<BaseResponse>();

            //        if (baseResponseData.ErrorCode == 143)
            //        {
            //            await RefreshToken();
            //            return await ChangePassword(changepwVM); ///!
            //        }
            //        else if (baseResponseData.ErrorCode == 0)
            //        {
            //            //TempData["Success"] = "Password chenged";
            //            //return View("~/Views/User/Settings.cshtml");

            //            return Json(new { StatusCode = 200, Message = "Password changed successfully" });
            //        }

            //        else
            //        {
            //            //TempData["Error"] = "Password unchenged";
            //            //return View("~/Views/User/Settings.cshtml");
            //            return Json(new { StatusCode = 500, Message = baseResponseData.ErrorMessage });
            //        }
            //    }
            //    else
            //    {
            //        // Обработка ошибки, если запрос не удался.
            //        return View("Error");
            //    }
            //}

        }


        [HttpGet]
        public async Task<IActionResult> GetQuestionnaires()
        {
            string token = GetToken();
            using (var httpClientForProfileInfo = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await GetQuestionnaires();
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForQuestionnaires = new HttpClient())
                        {

                            var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaires?token=" + token;

                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                            httpClientForQuestionnaires.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetQuestionnaires = await httpClientForQuestionnaires.GetAsync(apiUrlGetQuestionnairesByToken);

                            if (responseGetQuestionnaires.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<GetQuestionnairesInfo>();
                                if (questionnaireData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await GetQuestionnaires();
                                }
                                else if (questionnaireData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (questionnaireData.errorCode == 0)
                                {
                                    return PartialView("~/Views/User/_Questionnaire.cshtml", questionnaireData.questionnaires);
                                }
                            }
                        }
                    }
                }
            }
            return View("Error");
        }




    }
}
