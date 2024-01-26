using ISQuiz.Interface;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISQuiz.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger) : base(logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileInfo()
        {
            try
            {
                _logger.LogInformation("User.ProfileInfo method called.");

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ProfileInfo method.");
                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            try
            {
                _logger.LogInformation("User.Settings method called.");

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Settings method.");
                return PartialView("~/Views/_Shared/Error.cshtml");
            }
        }

        [HttpGet]
        public IActionResult ChangePassword() => PartialView("~/Views/User/_ChangePassword.cshtml");

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)
        {
            try
            {
                _logger.LogInformation("User.ChangePassword method called.");

                if (!ModelState.IsValid)
                    return PartialView("~/Views/User/_ChangePassword.cshtml", changepwVM);

                string token = GetToken();

                ChangePasswordViewModel changePassword = new()
                {
                    NewPassword = changepwVM.NewPassword,
                    OldPassword = changepwVM.OldPassword,
                    Token = token
                };

                var baseResponseData = await _userRepository.changePassword(changePassword);

                if (baseResponseData.ErrorCode == 0)
                    return Json(new { StatusCode = 200/*, Message = "Password changed successfully" */});
                else if (baseResponseData.ErrorCode == 143)
                {
                    await RefreshToken();
                    return await ChangePassword(changepwVM);
                }
                else
                    return Json(new { StatusCode = 500, Message = baseResponseData.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ChangePassword method.");
                return Json(new { StatusCode = 500, Message = "An error occurred while processing your request." });
            }
        }

    }
}
