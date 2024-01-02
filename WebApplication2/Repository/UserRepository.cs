using ISQuiz.Interface;
using ISQuiz.Models;
using ISQuiz.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace ISQuiz.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly HttpClient _httpClient;

        public UserRepository()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/")
            };
        }

        private async Task<T> SendPostRequest<T>(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, jsonContent);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        private async Task<T> SendGetRequest<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        public async Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM)
            => await SendPostRequest<BaseResponse>("ChangePassword", changePasswordVM);

        public async Task<GetProfileInfo> getProfileInfo(string token)
            => await SendGetRequest<GetProfileInfo>($"GetProfileInfo?Token={token}");
    }

}
