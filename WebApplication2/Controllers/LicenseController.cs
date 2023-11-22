using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using WebApplication2.Interface;
using WebApplication2.Models;
using WebApplication2.Models.API;
using WebApplication2.Models.API.License;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{

    [Authorize]
    public class LicenseController : BaseController
    {
        private readonly ILicenseRepository _licenseRepository;

        public LicenseController(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string token = GetToken();

            var licenseData = await _licenseRepository.GetLicenseList(token);

            if (licenseData.errorCode == 143)
            {
                await RefreshToken();
                return await Index();
            }
            else if (licenseData.errorCode == 0)
                return View("~/Views/License/Index.cshtml", licenseData.licenses);

            else /*if (licenseData.errorCode == 118)*/
                return View("~/Views/Account/Login.cshtml");
            

            //using (var httpClientForProfileInfo = new HttpClient())
            //{

            //    var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

            //    var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlForGetProfileInfo);

            //    if (responseGetProfileInfo.IsSuccessStatusCode)
            //    {
            //        var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
            //        if (userData.ErrorCode == 143)
            //        {
            //            await RefreshToken();
            //            return await Index();
            //        }
            //        else if (userData.ErrorCode == 118)
            //        {
            //            return View("~/Views/Account/Login.cshtml");
            //        }
            //        else if (userData.ErrorCode == 0)
            //        {
            //            using (var httpClientForLicenseList = new HttpClient())
            //            {

            //                var apiUrlGetLicenseListByToken = "https://dev.edi.md/ISNPSAPI/Web/GetLicenseList?token=" + token;

            //                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
            //                httpClientForLicenseList.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


            //                var responseGetLicenseList = await httpClientForLicenseList.GetAsync(apiUrlGetLicenseListByToken);

            //                if (responseGetLicenseList.IsSuccessStatusCode)
            //                {
            //                    var licensesData = await responseGetLicenseList.Content.ReadAsAsync<GetLicenseList>();
            //                    if (licensesData.errorCode == 143)
            //                    {
            //                        await RefreshToken();
            //                        return await Index();
            //                    }
            //                    else if (licensesData.errorCode == 118)
            //                    {
            //                        return View("~/Views/Account/Login.cshtml");
            //                    }
            //                    else if (licensesData.errorCode == 0)
            //                    {

            //                        return View("~/Views/License/Index.cshtml", licensesData.licenses);

            //                    }
            //                }
            //            }
            //        }

            //    }
            //}


            //return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            string token = GetToken();
            var licenseData = await _licenseRepository.GetLicense(token, id);
            if (licenseData.errorCode == 143)
            {
                await RefreshToken();
                return await Detail(id);
            }
            else if (licenseData.errorCode == 0)
                return PartialView("~/Views/License/Detail.cshtml", licenseData.license);
            else 
                return View("~/Views/Account/Login.cshtml");


            //using (var httpClientForProfileInfo = new HttpClient())
            //{

            //    var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

            //    var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlForGetProfileInfo);

            //    if (responseGetProfileInfo.IsSuccessStatusCode)
            //    {
            //        // Чтение данных из HTTP-ответа.

            //        var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
            //        if (userData.ErrorCode == 143)
            //        {
            //            await RefreshToken();
            //            return await Detail(id);
            //        }
            //        else if (userData.ErrorCode == 118)
            //        {
            //            return View("~/Views/Account/Login.cshtml");
            //        }
            //        else if (userData.ErrorCode == 0)
            //        {
            //            using (var httpClientForLicense = new HttpClient())
            //            {

            //                var apiUrlGetLicense = "https://dev.edi.md/ISNPSAPI/Web/GetLicense?token=" + token + "&Oid=" + id;

            //                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
            //                httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

            //                var responseGetLicense = await httpClientForLicense.GetAsync(apiUrlGetLicense);

            //                if (responseGetLicense.IsSuccessStatusCode)
            //                {
            //                    var questionnaireData = await responseGetLicense.Content.ReadAsAsync<GetLicense>();
            //                    if (questionnaireData.errorCode == 143)
            //                    {
            //                        await RefreshToken();
            //                        return await Detail(id);
            //                    }
            //                    else if (questionnaireData.errorCode == 118)
            //                    {
            //                        return View("~/Views/Account/Login.cshtml");
            //                    }
            //                    else if (questionnaireData.errorCode == 0)
            //                    {

            //                        return PartialView("~/Views/License/Detail.cshtml", questionnaireData.license);

            //                    }
            //                }
            //            }
            //        }

            //    }
            //}


            //return View("Error");
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
            string token = GetToken();

            var licenseResponse = await _licenseRepository.DeactivateLicense(token, oid);
            if (licenseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await Deactivate(oid);
            }
            else if (licenseResponse.errorCode == 118)
                return View("~/Views/Account/Login.cshtml");
            else if (licenseResponse.errorCode == 0)
                return Json(new { StatusCode = 200, Message = "Ok" });
            else
                return Json(new { StatusCode = 500, Message = licenseResponse.errorMessage });
            /*try
            {
                string token = GetToken();


                using (var httpClientForDeactivateLicense = new HttpClient())
                {

                    var apiUrlDeactivateLicense = "https://dev.edi.md/ISNPSAPI/Web/DeactivateLicense?token=" + token + "&Oid=" + oid;

                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                    httpClientForDeactivateLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                    var responseDeactivateLicense = await httpClientForDeactivateLicense.GetAsync(apiUrlDeactivateLicense);

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
                            return Json(new { StatusCode = 200, Message = "Ok" });

                        }
                        else
                        {
                            return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                        }
                    }
                }
                        
            }
            catch (Exception ex)
            {

            }
            return View("Error");
            */
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string oid)
        {

            string token = GetToken();
            var licenseResponse = await _licenseRepository.ActivateLicense(token, oid);
            if (licenseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await Deactivate(oid);
            }
            else if (licenseResponse.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (licenseResponse.errorCode == 0)
            {
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            else
            {
                return Json(new { StatusCode = 500, Message = licenseResponse.errorMessage });
            }

            /*using (var httpClientForProfileInfo = new HttpClient())
            {

                var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlForGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
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
                        using (var httpClientForActivateLicense = new HttpClient())
                        {

                            var apiUrlActivateLicense = "https://dev.edi.md/ISNPSAPI/Web/ActivateLicense?token=" + token + "&Oid=" + oid;

                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                            httpClientForActivateLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                            var responseActivateLicense = await httpClientForActivateLicense.GetAsync(apiUrlActivateLicense);

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
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                                }
                            }
                        }
                    }
                }
            }
            return View("Error");*/
        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {
            string token = GetToken();
            var licenseResponse =  await _licenseRepository.ReleaseLicense(token, oid);
            if (licenseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await Deactivate(oid);
            }
            else if (licenseResponse.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (licenseResponse.errorCode == 0)
            {
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            else
            {
                return Json(new { StatusCode = 500, Message = licenseResponse.errorMessage });
            }
            /*
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
                        return await Deactivate(oid);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForReleaseLicense = new HttpClient())
                        {

                            var apiUrlReleaseLicense = "https://dev.edi.md/ISNPSAPI/Web/ReleaseLicense?token=" + token + "&Oid=" + oid;

                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                            httpClientForReleaseLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                            var responseReleaseLicense = await httpClientForReleaseLicense.GetAsync(apiUrlReleaseLicense);

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
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    return Json(new { StatusCode = 500, Message = licenseData.errorMessage });
                                }
                            }
                        }
                    }
                }
            }
            return View("Error");*/
        }


        [HttpGet]
        public async Task<IActionResult> CreateLicence()
        {
            return PartialView("~/Views/License/_CreateLicence.cshtml");
        }

        [HttpPost] //418Line PostAsync?
        public async Task<IActionResult> CreateLicence(GenerateLicenseViewModel generateLicenseVM)
        {
            if (generateLicenseVM.quantity > 0)
            {
                string token = GetToken();
                generateLicenseVM.token = token;
                var postLicenseBaseResponse = await _licenseRepository.GenerateLicense(generateLicenseVM);
                if (postLicenseBaseResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await CreateLicence(generateLicenseVM); ///!
                }
                else if (postLicenseBaseResponse.errorCode == 0)
                {
                    return RedirectToAction(nameof(LicenseController.Index), "License");
                }
                /* using (var httpClientForProfileInfo = new HttpClient())
                 {

                     var apiUrlForGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                     var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlForGetProfileInfo);

                     if (responseGetProfileInfo.IsSuccessStatusCode)
                     {
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
                             using (var httpClientForPostLicense = new HttpClient())
                             {
                                 var addingStr = "?token=" + token + "&quantity=" + generateLicenseVM.quantity;

                                 var apiUrlForPostLicense = "https://dev.edi.md/ISNPSAPI/Web/GenerateLicense" + addingStr;

                                 var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                                 httpClientForPostLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
                                 //generateLicenseVM.token = token;

                                 var jsonContent = new StringContent(JsonConvert.SerializeObject(generateLicenseVM), Encoding.UTF8, "application/json"); //Зачем мне передовать объект

                                 var responsePostLicense = await httpClientForPostLicense.PostAsync(apiUrlForPostLicense, jsonContent);

                                 if (responsePostLicense.IsSuccessStatusCode)
                                 {
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
                 }*/
            }

            return View("Error");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return PartialView("~/Views/License/Delete.cshtml", id);
            /*
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
                        return await Delete(id);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        return PartialView("~/Views/License/Delete.cshtml", id);
                    }
                }
            }
            return View("Error");*/
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLicense(string oid)
        {
            string token = GetToken();
            var deleteLicenseBaseResponse = await _licenseRepository.Delete(token, oid);
            if (deleteLicenseBaseResponse.errorCode == 143)
            {
                await RefreshToken();
                return await DeleteLicense(oid);
            }
            else if (deleteLicenseBaseResponse.errorCode == 118)
            {
                return View("~/Views/Account/Login.cshtml");
            }
            else if (deleteLicenseBaseResponse.errorCode == 0)
            {
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            else
            {
                return Json(new { StatusCode = 500, Message = deleteLicenseBaseResponse.errorMessage });
            }

            /*
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
                        return await DeleteLicense(oid);
                    }
                    else if (userData.ErrorCode == 118)
                    {
                        return View("~/Views/Account/Login.cshtml");
                    }
                    else if (userData.ErrorCode == 0)
                    {
                        using (var httpClientForDeleteLicense = new HttpClient())
                        {

                            var apiUrlDeleteLicense = "https://dev.edi.md/ISNPSAPI/Web/DeleteLicense?token=" + token + "&Oid=" + oid;

                            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                            httpClientForDeleteLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                            var responseDeleteLicense = await httpClientForDeleteLicense.DeleteAsync(apiUrlDeleteLicense);

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
                                    return Json(new { StatusCode = 200, Message = "Ok" });

                                }
                                else
                                {
                                    return Json(new { StatusCode = 500, Message = licenseDataError.errorMessage });
                                }
                            }
                        }
                    }
                }
            }
            return View("Error");
            */
        }

    }
}
