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

            return View("~/Views/User/ProfileInfo.cshtml", UserData.User);

        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {



            string token = GetToken();
            var UserData = await _userRepository.getProfileInfo(token);


            return View("~/Views/User/Settings.cshtml", UserData.User);

        }

        [HttpGet]
        public IActionResult ChangePassword() => PartialView("~/Views/User/_ChangePassword.cshtml");

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeConfirmPasswordViewModel changepwVM)
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
            return Json(new { StatusCode = 200/*, Message = "Password changed successfully" */});

        }

    }
}
