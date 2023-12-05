using WebApplication2.Models;
using WebApplication2.Models.Enum;
using WebApplication2.ViewModels;

namespace WebApplication2.Interface
{
    public interface IAccountRepository
    {
        Task<BaseResponse> RecoverPassword(AuthRecoverpwViewModel recoverpwVM);
        Task<GetProfileInfo> AuthorizeUser(AuthorizeViewModel loginVM);
        Task<BaseResponse> ChangeUILanguage(string token, EnUiLanguage uiLanguage);

    }
}
