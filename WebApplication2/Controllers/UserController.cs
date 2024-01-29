using ISQuiz.Interface;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ISQuiz.Controllers
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
            //Log.Information("Into User.ProfileInfo");
            try
            {
                string token = GetToken();
                var UserData = await _userRepository.getProfileInfo(token);

                if (UserData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await ProfileInfo();
                }
                else if (UserData.ErrorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@UserData}", UserData);
                    throw new Exception(UserData.ErrorMessage);
                }

                return View("~/Views/User/ProfileInfo.cshtml", UserData.User);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml");
            }


        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            //Log.Information("Into User.Settings");
            try
            {
                string token = GetToken();
                var UserData = await _userRepository.getProfileInfo(token);


                if (UserData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await Settings();
                }
                else if (UserData.ErrorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@UserData}", UserData);
                    throw new Exception(UserData.ErrorMessage);
                }

                return View("~/Views/User/Settings.cshtml", UserData.User);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return PartialView("~/Views/_Shared/Error.cshtml");
            }

        }

        [HttpGet]
        public IActionResult ChangePassword() => PartialView("~/Views/User/_ChangePassword.cshtml");

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)
        {
            //Log.Information("Into User.ChangePassword");
            try
            {
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
                if (baseResponseData.ErrorCode == EnErrorCode.Expired_token)
                {
                    if (await RefreshToken()) return await ChangePassword(changepwVM);
                }
                else if (baseResponseData.ErrorCode != EnErrorCode.NoError)
                {
                    Log.Information("Response => {@baseResponseData}", baseResponseData);
                    throw new Exception(baseResponseData.ErrorMessage);
                }
                return Json(new { StatusCode = 200/*, Message = "Password changed successfully" */});
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message); 
                return Json(new { StatusCode = 500, Message = ex.Message});
                //return View("~/Views/_Shared/Error.cshtml");
            }

        }

    }
}
