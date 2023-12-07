using Newtonsoft.Json;
using System.Text;
using WebApplication2.Interface;
using WebApplication2.Models;
using WebApplication2.Models.Enum;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<GetProfileInfo> AuthorizeUser(AuthorizeViewModel loginVM)
        {
            using (var httpClientForAuth = new HttpClient())
            {
                var apiUrlForAuth = "https://dev.edi.md/ISAuthService/json/AuthorizeUser";

                var jsonContent = new StringContent(JsonConvert.SerializeObject(loginVM), Encoding.UTF8, "application/json");

                var responseAuth = await httpClientForAuth.PostAsync(apiUrlForAuth, jsonContent);

                if (responseAuth.IsSuccessStatusCode)
                    return await responseAuth.Content.ReadAsAsync<GetProfileInfo>();

                return new GetProfileInfo { ErrorCode = -1 };

            }
        }

        public async Task<BaseResponse> ChangeUILanguage(string token, EnUiLanguage uiLanguage)
        {
            using (var httpClientForChangeLanguage = new HttpClient())
            {
                var apiUrlUserLanguage = "https://dev.edi.md/ISAuthService/json/ChangeUILanguage?Token=" + token + "&Language=" + uiLanguage;

                var responseUserLanguage = await httpClientForChangeLanguage.GetAsync(apiUrlUserLanguage);
                if (responseUserLanguage.IsSuccessStatusCode)
                    return await responseUserLanguage.Content.ReadAsAsync<BaseResponse>();
                return new BaseResponse { ErrorCode = -1 };
            }
        }

        public async Task<BaseResponse> RecoverPassword(AuthRecoverpwViewModel recoverpwVM)
        {
            using (var httpClientForResetPW = new HttpClient())
            {

                var apiUrlForResetPW = "https://dev.edi.md/ISAuthService/json/ResetPassword";

                var jsonContent = new StringContent(JsonConvert.SerializeObject(recoverpwVM), Encoding.UTF8, "application/json");

                var response = await httpClientForResetPW.PostAsync(apiUrlForResetPW, jsonContent);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<BaseResponse>();

                return new BaseResponse() { ErrorCode = -1 };
            }
        }
    }
}
