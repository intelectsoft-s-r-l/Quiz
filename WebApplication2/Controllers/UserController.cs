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


        [HttpGet]
        public IActionResult ChangePassword() => PartialView("~/Views/User/_ChangePassword.cshtml");


        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)
        {

            if (!ModelState.IsValid)
            {
                /* var errors = ModelState.Where(x => x.Value.Errors.Any())
                                        .ToDictionary(
                                             kvp => kvp.Key,
                                             kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
                */
                //Console.WriteLine($"Validation Errors: {string.Join(", ", errors.Values.SelectMany(x => x))}");

                //return Json(new { StatusCode = 500, Message = Localization.UnSuccessPW/*string.Join(", ", errors.Values.SelectMany(x => x))*/ });
                return PartialView("~/Views/User/_ChangePassword.cshtml", changepwVM);
            }




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
    }
}
