using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using WebApplication2.Models;
using WebApplication2.Models.API;
using WebApplication2.Models.API.License;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{

    [Authorize]
    public class LicenseController : BaseController
    {

        [HttpGet]
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
                        using (var httpClientForLicenseList = new HttpClient())
                        {

                            var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/GetLicenseList?token=" + token;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClientForLicenseList.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetLicenseList = await httpClientForLicenseList.GetAsync(apiUrlGetQuestionnairesByToken);

                            if (responseGetLicenseList.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetLicenseList.Content.ReadAsAsync<GetLicenseList>();
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

                                    return View("~/Views/License/Index.cshtml", questionnaireData.licenses);

                                }
                            }
                        }
                    }



                }
            }


            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
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
                        return await Detail(id);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForLicense = new HttpClient())
                        {

                            var apiUrlGetLicense = "https://dev.edi.md/ISNPSAPI/Web/GetLicense?token=" + token + "&Oid=" + id;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetLicense = await httpClientForLicense.GetAsync(apiUrlGetLicense);

                            if (responseGetLicense.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetLicense.Content.ReadAsAsync<GetLicense>();
                                if (questionnaireData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Detail(id);
                                }
                                else if (questionnaireData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (questionnaireData.errorCode == 0)
                                {

                                    return PartialView("~/Views/License/Detail.cshtml", questionnaireData.license);

                                }
                            }
                        }
                    }



                }
            }


            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> Action(string oid, int option)
        {
            if (oid != null && option > 0)
            {
                if (option == 1)
                    return PartialView("~/Views/License/_Activate.cshtml", oid);
                if (option == 2)
                    return PartialView("~/Views/License/_Deactivate.cshtml", oid);
                if (option == 3)
                    return PartialView("~/Views/License/_Release.cshtml", oid);
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(string oid)
        {
            try
            {
                string token = GetToken();

                using (var httpClientForUser = new HttpClient())
                {

                    var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                    var responseGetProfileInfo = await httpClientForUser.GetAsync(apiUrlForGetProfileInfo);

                    if (responseGetProfileInfo.IsSuccessStatusCode)
                    {
                        // Чтение данных из HTTP-ответа.

                        var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                        if (userData.ErrorCode == 143)
                        {
                            await RefreshToken();
                            return await Deactivate(oid);
                        }
                        else if (userData.ErrorCode == 118)
                        {
                            return View("~/Views/Account/Login.cshtml");
                        }
                        else if (userData.ErrorCode == 0)
                        {
                            using (var httpClientForLicense = new HttpClient())
                            {

                                var apiUrlDeactivateLicense = "https://dev.edi.md/ISNPSAPI/Web/DeactivateLicense?token=" + token + "&Oid=" + oid;
                                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                                // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                                httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                                var responseDeactivateLicense = await httpClientForLicense.GetAsync(apiUrlDeactivateLicense);

                                if (responseDeactivateLicense.IsSuccessStatusCode)
                                {
                                    var licenseData = await responseDeactivateLicense.Content.ReadAsAsync<BaseErrors>();
                                    if (licenseData.errorCode == 143)
                                    {
                                        await RefreshToken();
                                        return await Deactivate(oid);
                                    }
                                    else if (licenseData.errorCode == 118)
                                    {
                                        return View("~/Views/Account/Login.cshtml");
                                    }
                                    else if (licenseData.errorCode == 0)
                                    {

                                        // return PartialView("~/Views/Home/Index.cshtml");
                                        return Json(new { StatusCode = 200, Message = "Ok" });

                                    }
                                    else
                                    {
                                        //TempData["Error"] = "Password unchenged";
                                        //return View("~/Views/User/Settings.cshtml");
                                        return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                                    }
                                }
                            }
                        }



                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string oid)
        {

            string token = GetToken();

            using (var httpClientForUser = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClientForUser.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Deactivate(oid);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForLicense = new HttpClient())
                        {

                            var apiUrlActivateLicense = "https://dev.edi.md/ISNPSAPI/Web/ActivateLicense?token=" + token + "&Oid=" + oid;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseActivateLicense = await httpClientForLicense.GetAsync(apiUrlActivateLicense);

                            if (responseActivateLicense.IsSuccessStatusCode)
                            {
                                var licenseData = await responseActivateLicense.Content.ReadAsAsync<BaseErrors>();
                                if (licenseData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Deactivate(oid);
                                }
                                else if (licenseData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (licenseData.errorCode == 0)
                                {

                                    // return PartialView("~/Views/Home/Index.cshtml");
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    //TempData["Error"] = "Password unchenged";
                                    //return View("~/Views/User/Settings.cshtml");
                                    return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                                }
                            }
                        }
                    }



                }
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {
            string token = GetToken();

            using (var httpClientForUser = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClientForUser.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    // Чтение данных из HTTP-ответа.

                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    if (userData.ErrorCode == 143)
                    {
                        await RefreshToken();
                        return await Deactivate(oid);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForLicense = new HttpClient())
                        {

                            var apiUrlReleaseLicense = "https://dev.edi.md/ISNPSAPI/Web/ReleaseLicense?token=" + token + "&Oid=" + oid;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseReleaseLicense = await httpClientForLicense.GetAsync(apiUrlReleaseLicense);

                            if (responseReleaseLicense.IsSuccessStatusCode)
                            {
                                var licenseData = await responseReleaseLicense.Content.ReadAsAsync<BaseErrors>();
                                if (licenseData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Deactivate(oid);
                                }
                                else if (licenseData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (licenseData.errorCode == 0)
                                {

                                    // return PartialView("~/Views/Home/Index.cshtml");
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    //TempData["Error"] = "Password unchenged";
                                    //return View("~/Views/User/Settings.cshtml");
                                    return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                                }
                            }
                        }
                    }



                }
            }
            return View("Error");
        }



        [HttpGet]
        public async Task<IActionResult> CreateLicence()
        {
            return PartialView("~/Views/License/_CreateLicence.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateLicence(GenerateLicenseViewModel generateLicenseVM)
        {
            if (generateLicenseVM.quantity > 0)
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
                            return await CreateLicence(generateLicenseVM);
                        }
                        else if (userData.ErrorCode == 118)
                        {
                            return View("~/Views/Account/Login.cshtml");
                        }
                        else if (userData.ErrorCode == 0)
                        {
                            using (var httpClient1 = new HttpClient())
                            {
                                var addingStr = "?token=" + token + "&quantity=" + generateLicenseVM.quantity;
                                var apiUrlForPostLicense = "https://dev.edi.md/ISNPSAPI/Web/GenerateLicense" + addingStr;
                                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                                // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                                httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
                                //generateLicenseVM.token = token;

                                var jsonContent = new StringContent(JsonConvert.SerializeObject(generateLicenseVM), Encoding.UTF8, "application/json");

                                var responsePostLicense = await httpClient1.PostAsync(apiUrlForPostLicense, jsonContent);

                                if (responsePostLicense.IsSuccessStatusCode)
                                {
                                    // Чтение данных из HTTP-ответа.

                                    var baseResponsedData = await responsePostLicense.Content.ReadAsAsync<BaseErrors>();

                                    if (baseResponsedData.errorCode == 143)
                                    {
                                        await RefreshToken();
                                        return await CreateLicence(generateLicenseVM); ///!
                                    }
                                    else if (baseResponsedData.errorCode == 0)
                                    {
                                        return RedirectToAction(nameof(LicenseController.Index), "License");
                                    }



                                }
                            }
                        }
                    }
                }
            }

            return View("~/Views/Home/Index.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
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
                        return await Delete(id);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        return PartialView("~/Views/License/Delete.cshtml", id);
                        /*using (var httpClient1 = new HttpClient())
                        {

                            var apiUrlGetQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaire?Token=" + token + "&id=" + id;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseGetQuestionnaires = await httpClient1.GetAsync(apiUrlGetQuestionnaire);

                            if (responseGetQuestionnaires.IsSuccessStatusCode)
                            {
                                var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<DetailQuestionnaire>();
                                if (questionnaireData.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await Delete(id);
                                }
                                else if (questionnaireData.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (questionnaireData.errorCode == 0)
                                {

                                    return PartialView("~/Views/Home/Delete.cshtml", questionnaireData);

                                }
                            }
                        }*/
                    }
                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLicense(string oid)
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
                        return await DeleteLicense(oid);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClient1 = new HttpClient())
                        {

                            var apiUrlDeleteLicense = "https://dev.edi.md/ISNPSAPI/Web/DeleteLicense?token=" + token + "&Oid=" + oid;
                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                            // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                            httpClient1.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseDeleteLicense = await httpClient1.DeleteAsync(apiUrlDeleteLicense);

                            if (responseDeleteLicense.IsSuccessStatusCode)
                            {
                                var licenseDataError = await responseDeleteLicense.Content.ReadAsAsync<BaseErrors>();
                                if (licenseDataError.errorCode == 143)
                                {
                                    await RefreshToken();
                                    return await DeleteLicense(oid);
                                }
                                else if (licenseDataError.errorCode == 118)
                                {
                                    return View("~/Views/Account/Login.cshtml");
                                }
                                else if (licenseDataError.errorCode == 0)
                                {

                                    // return PartialView("~/Views/Home/Index.cshtml");
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    //TempData["Error"] = "Password unchenged";
                                    //return View("~/Views/User/Settings.cshtml");
                                    return Json(new { StatusCode = 500, Message = licenseDataError.errorMessage });
                                }



                            }
                        }
                    }



                }
            }
            return View("Index");
        }

    }
}
