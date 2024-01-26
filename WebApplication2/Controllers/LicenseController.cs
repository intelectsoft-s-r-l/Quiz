using ISQuiz.Interface;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            string token = GetToken();

            var licenseData = await _licenseRepository.GetLicenseList(token);
            return View("~/Views/License/Index.cshtml", licenseData.licenses);


        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {

            string token = GetToken();
            var licenseData = await _licenseRepository.GetLicense(token, id);
            return PartialView("~/Views/License/Detail.cshtml", licenseData.license);

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

            string token = GetToken();

            var licenseResponse = await _licenseRepository.DeactivateLicense(token, oid);
            return Json(new { StatusCode = 200, Message = "Ok" });


        }



        [HttpGet]
        public async Task<IActionResult> Activate(string oid)
        {

            string token = GetToken();
            var licenseResponse = await _licenseRepository.ActivateLicense(token, oid);
            return Json(new { StatusCode = 200, Message = "Ok" });



        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {

            string token = GetToken();
            var licenseResponse = await _licenseRepository.ReleaseLicense(token, oid);
            return Json(new { StatusCode = 200, Message = "Ok" });



        }


        [HttpGet]
        public IActionResult CreateLicence() => PartialView("~/Views/License/_CreateLicence.cshtml");


        [HttpPost]
        public async Task<IActionResult> CreateLicence([FromBody] GenerateLicenseViewModel generateLicenseVM)
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
                return Json(new { statusCode = 200 });
            }


        }


        [HttpGet]
        public IActionResult Delete(string id) => PartialView("~/Views/License/Delete.cshtml", id);

        [HttpPost]
        public async Task<IActionResult> DeleteLicense(string oid)
        {
            string token = GetToken();
            var deleteLicenseBaseResponse = await _licenseRepository.Delete(token, oid);
            return Json(new { StatusCode = 200, Message = "Ok" });




        }

    }
}
