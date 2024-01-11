using ISQuiz.Interface;
using ISQuiz.Models;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace ISQuiz.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly HttpClient _httpClient;

        public AccountRepository()
        {
            _httpClient = CreateHttpClient();
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/")
            };
            // Добавьте другие настройки, если они необходимы, например, таймауты, заголовки и т. д.
            return httpClient;
        }

        private async Task<T> SendRequest<T>(HttpMethod method, string endpoint, object data = null)
        {
            using var requestMessage = new HttpRequestMessage(method, endpoint);
            if (data != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                // добавить логирование и более детальную обработку ошибок
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }

            return await response.Content.ReadAsAsync<T>();
        }

        // GET
        public async Task<BaseResponse> ChangeUILanguage(string token, EnUiLanguage uiLanguage)
        {
            var endpoint = $"ChangeUILanguage?Token={token}&Language={uiLanguage}";
            return await SendRequest<BaseResponse>(HttpMethod.Get, endpoint);
        }

        // POST
        public async Task<GetProfileInfo> AuthorizeUser(AuthorizeViewModel loginVM)
            => await SendRequest<GetProfileInfo>(HttpMethod.Post, "AuthorizeUser", loginVM);

        public async Task<BaseResponse> RecoverPassword(AuthRecoverpwViewModel recoverpwVM)
            => await SendRequest<BaseResponse>(HttpMethod.Post, "ResetPassword", recoverpwVM);

    }

}
