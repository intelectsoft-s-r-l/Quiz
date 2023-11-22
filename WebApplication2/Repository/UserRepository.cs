using Newtonsoft.Json;
using System.Text;
using WebApplication2.Interface;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM)
        {
            using (var httpClientForChangePW = new HttpClient())
            {

                var apiUrlForChangePW = "https://dev.edi.md/ISAuthService/json/ChangePassword";

                var jsonContent = new StringContent(JsonConvert.SerializeObject(changePasswordVM), Encoding.UTF8, "application/json");

                var responseChangePW = await httpClientForChangePW.PostAsync(apiUrlForChangePW, jsonContent);

                if (responseChangePW.IsSuccessStatusCode)
                {
                    var baseResponseData = await responseChangePW.Content.ReadAsAsync<BaseResponse>();

                    return baseResponseData;
                }
                return new BaseResponse() { ErrorCode = -1 };
            }
        }

        public async Task<GetProfileInfo> getProfileInfo(string token)
        {
            using (var httpClientForProfileInfo = new HttpClient())
            {
                var apiUrlGetProfileInfo = "https://dev.edi.md/ISAuthService/json/GetProfileInfo?Token=" + token;

                var responseGetProfileInfo = await httpClientForProfileInfo.GetAsync(apiUrlGetProfileInfo);

                if (responseGetProfileInfo.IsSuccessStatusCode)
                {
                    var userData = await responseGetProfileInfo.Content.ReadAsAsync<GetProfileInfo>();
                    return userData;
                }
                return new GetProfileInfo() { ErrorCode = -1 }; //
            }
        }
    }
}
