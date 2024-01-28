using ISQuiz.Interface;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ISQuiz.Controllers
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
            try
            {
                string token = GetToken();

                var licenseData = await _licenseRepository.GetLicenseList(token);

                if (licenseData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Index();
                }
                else if (licenseData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(licenseData.errorMessage);
                }

                return View("~/Views/License/Index.cshtml", licenseData.licenses);


            }
            catch (Exception ex)
            {
                return PartialView("~/Views/_Shared/Error.cshtml");
            }


        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                string token = GetToken();
                var licenseData = await _licenseRepository.GetLicense(token, id);


                if (licenseData.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Detail(id);
                }
                else if (licenseData.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(licenseData.errorMessage);
                }


                return PartialView("~/Views/License/Detail.cshtml", licenseData.license);

            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }


        [HttpGet]
        public IActionResult Action(string oid, int option)
        {
            if (!string.IsNullOrEmpty(oid) && option > 0)
            {
                if (option == 1)
                    return PartialView("~/Views/License/_Activate.cshtml", oid);
                if (option == 2)
                    return PartialView("~/Views/License/_Deactivate.cshtml", oid);
                if (option.Equals(3))
                    return PartialView("~/Views/License/_Release.cshtml", oid);
            }

            return PartialView("~/Views/_Shared/Error.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(string oid)
        {

            try
            {
                string token = GetToken();

                var licenseResponse = await _licenseRepository.DeactivateLicense(token, oid);

                if (licenseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Deactivate(oid);
                }
                else if (licenseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(licenseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, Message = "Ok" });

            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }


        }



        [HttpGet]
        public async Task<IActionResult> Activate(string oid)
        {
            try
            {

                string token = GetToken();
                var licenseResponse = await _licenseRepository.ActivateLicense(token, oid);
                if (licenseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Activate(oid);
                }
                else if (licenseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(licenseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, Message = "Ok" });
            }
            catch (Exception ex)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {
            try
            {
                string token = GetToken();
                var licenseResponse = await _licenseRepository.ReleaseLicense(token, oid);

                if (licenseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Release(oid);
                }
                else if (licenseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(licenseResponse.errorMessage);
                }
                return Json(new { StatusCode = 200, Message = "Ok" });
            }
            catch (Exception)
            {
                return PartialView("~/Views/_Shared/Error.cshtml");
            }




        }


        [HttpGet]
        public IActionResult CreateLicence() => PartialView("~/Views/License/_CreateLicence.cshtml");


        [HttpPost]
        public async Task<IActionResult> CreateLicence([FromBody] GenerateLicenseViewModel generateLicenseVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/License/_CreateLicence.cshtml", generateLicenseVM);
                }
                else
                {
                    string token = GetToken();
                    generateLicenseVM.token = token;
                    var postLicenseBaseResponse = await _licenseRepository.GenerateLicense(generateLicenseVM);
                    if (postLicenseBaseResponse.errorCode == EnErrorCode.Expired_token)
                    {
                        if (await RefreshToken()) return await CreateLicence(generateLicenseVM);
                    }
                    else if (postLicenseBaseResponse.errorCode != EnErrorCode.NoError)
                    {
                        throw new Exception(postLicenseBaseResponse.errorMessage);
                    }

                    return Json(new { statusCode = 200 });
                }
            }
            catch (Exception)
            {

                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }


        [HttpGet]
        public IActionResult Delete(string id) => PartialView("~/Views/License/Delete.cshtml", id);

        [HttpDelete]
        public async Task<IActionResult> DeleteLicense(string oid)
        {
            try
            {
                string token = GetToken();
                var deleteLicenseBaseResponse = await _licenseRepository.Delete(token, oid);
                if (deleteLicenseBaseResponse.errorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await DeleteLicense(oid);
                }
                else if (deleteLicenseBaseResponse.errorCode != EnErrorCode.NoError)
                {
                    throw new Exception(deleteLicenseBaseResponse.errorMessage);
                }

                return Json(new { StatusCode = 200, Message = "Ok" });


            }
            catch (Exception ex)
            {
                
                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }

    }
}
