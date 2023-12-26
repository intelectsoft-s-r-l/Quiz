using ISQuiz.Models;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;

namespace ISQuiz.Interface
{
    public interface IAccountRepository
    {
        Task<BaseResponse> RecoverPassword(AuthRecoverpwViewModel recoverpwVM);
        Task<GetProfileInfo> AuthorizeUser(AuthorizeViewModel loginVM);
        Task<BaseResponse> ChangeUILanguage(string token, EnUiLanguage uiLanguage);

    }
}
