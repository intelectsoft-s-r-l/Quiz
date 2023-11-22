using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Interface
{
    public interface IUserRepository
    {
        Task<GetProfileInfo> getProfileInfo(string token);
        Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM);

    }
}
