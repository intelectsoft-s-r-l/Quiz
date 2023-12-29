using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ISQuiz.Interface;
using ISQuiz.ViewModels;

namespace ISQuiz.Controllers
{

    [Authorize]
    public class LicenseController : BaseController
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILogger<LicenseController> _logger;

        public LicenseController(ILicenseRepository licenseRepository, ILogger<LicenseController> logger)
        {
            _licenseRepository = licenseRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"License.Index method called.");
            try
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

                else
                {/*if (licenseData.errorCode == 118)*/
                    _logger.LogError($"{licenseData}");
                    return View("~/Views/Account/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.Index method." + ex.Message);
                throw;
            }



        }


        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            _logger.LogInformation($"License.Detail method called.");
            try
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
                {
                    _logger.LogError($"{licenseData}");
                    return View("~/Views/Account/Login.cshtml");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.Detail method." + ex.Message);
                throw;
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
                if (option == 3)
                    return PartialView("~/Views/License/_Release.cshtml", oid);
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(string oid)
        {
            _logger.LogInformation($"License.Deactivate method called.");
            try
            {
                string token = GetToken();

                var licenseResponse = await _licenseRepository.DeactivateLicense(token, oid);
                if (licenseResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await Deactivate(oid);
                }
                else if (licenseResponse.errorCode == 0)
                    return Json(new { StatusCode = 200, Message = "Ok" });
                else
                {
                    _logger.LogError($"{licenseResponse}");
                    return View("~/Views/Account/Login.cshtml");
                }
                /* else
                     return Json(new { StatusCode = 500, Message = licenseResponse.errorMessage });
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.Deactivate method." + ex.Message);
                throw;
            }


        }

        [HttpGet]
        public async Task<IActionResult> Activate(string oid)
        {
            _logger.LogInformation($"License.Activate method called.");
            try
            {
                string token = GetToken();
                var licenseResponse = await _licenseRepository.ActivateLicense(token, oid);
                if (licenseResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await Deactivate(oid);
                }
                else if (licenseResponse.errorCode == 0)
                {
                    return Json(new { StatusCode = 200, Message = "Ok" });

                }
                else
                {
                    _logger.LogError($"{licenseResponse}");
                    return View("~/Views/Account/Login.cshtml");
                }
                //else
                //{
                //    return Json(new { StatusCode = 500, Message = licenseResponse.errorMessage });
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.Activate method." + ex.Message);
                throw;
            }



        }

        [HttpGet]
        public async Task<IActionResult> Release(string oid)
        {
            _logger.LogInformation($"License.Release method called.");
            try
            {
                string token = GetToken();
                var licenseResponse = await _licenseRepository.ReleaseLicense(token, oid);
                if (licenseResponse.errorCode == 143)
                {
                    await RefreshToken();
                    return await Deactivate(oid);
                }
                else if (licenseResponse.errorCode == 0)
                {
                    return Json(new { StatusCode = 200, Message = "Ok" });

                }
                else
                {
                    _logger.LogError($"{licenseResponse}");
                    return View("~/Views/Account/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.Release method." + ex.Message);
                throw;
            }


        }


        [HttpGet]
        public IActionResult CreateLicence() => PartialView("~/Views/License/_CreateLicence.cshtml");


        [HttpPost]
        public async Task<IActionResult> CreateLicence([FromBody] GenerateLicenseViewModel generateLicenseVM)
        {
            _logger.LogInformation($"License.CreateLicence method called.");
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
                    if (postLicenseBaseResponse.errorCode == 143)
                    {
                        await RefreshToken();
                        return await CreateLicence(generateLicenseVM); ///!
                    }
                    else if (postLicenseBaseResponse.errorCode != 0)
                    {
                        _logger.LogError($"{postLicenseBaseResponse}");
                        return View("~/Views/Account/Login.cshtml");
                    }
                    //return RedirectToAction(nameof(LicenseController.Index), "License");
                    return Json(new { StatusCode = 200, Message = "Ok" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.CreateLicence method." + ex.Message);
                throw;
            }
            
        }


        [HttpGet]
        public IActionResult Delete(string id) => PartialView("~/Views/License/Delete.cshtml", id);

        [HttpPost]
        public async Task<IActionResult> DeleteLicense(string oid)
        {
            _logger.LogInformation($"License.DeleteLicense method called.");
            try
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
                    _logger.LogError($"{deleteLicenseBaseResponse}");
                    return View("~/Views/Account/Login.cshtml");
                    //return Json(new { StatusCode = 500, Message = deleteLicenseBaseResponse.errorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the License.DeleteLicense method." + ex.Message);
                throw;
            }



        }

    }
}
