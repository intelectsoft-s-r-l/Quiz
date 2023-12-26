using ISQuiz.Models;
using ISQuiz.ViewModels;

namespace ISQuiz.Interface
{
    public interface IUserRepository
    {
        Task<GetProfileInfo> getProfileInfo(string token);
        Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM);

    }
}
