using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interface;
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


        }


        [HttpGet]
        public IActionResult Action(string oid, int option)
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


        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {
            string token = GetToken();
            var licenseResponse = await _licenseRepository.ReleaseLicense(token, oid);
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

        }


        [HttpGet]
        public IActionResult CreateLicence()
        {
            return PartialView("~/Views/License/_CreateLicence.cshtml");
        }

        [HttpPost] //418Line PostAsync?
        public async Task<IActionResult> CreateLicence(GenerateLicenseViewModel generateLicenseVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(LicenseController.Index), "License");



            string token = GetToken();
                generateLicenseVM.token = token;
                var postLicenseBaseResponse = await _licenseRepository.GenerateLicense(generateLicenseVM);
                if (postLicenseBaseResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await CreateLicence(generateLicenseVM); ///!
                }
                else if (postLicenseBaseResponse.errorCode != 0)
                {
                return View("Error");
                }
            return RedirectToAction(nameof(LicenseController.Index), "License");

        }


        [HttpGet]
        public IActionResult Delete(string id)
        {
            return PartialView("~/Views/License/Delete.cshtml", id);

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


        }

    }
}
