using ISQuizBLL.Queries;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ISQuiz.Repository
{
    public class GlobalQuery
    {
#if (DEBUG)
        private string BaseURI = "https://dev.edi.md";
#endif
#if RELEASE
        private string BaseURI = "https://api.eservicii.md";
#endif

        public async Task<T> SendRequest<T>(QueryData queryData)
        {
            if (queryData.endpoint.Contains("ISNPSAPI"))
            {
#if DEBUG
                BaseURI = "https://dev.edi.md";
#elif RELEASE
            BaseURI = "https://survey.eservicii.md";
#endif
            }

            using var _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseURI + queryData.endpoint),
            };

            if (!string.IsNullOrEmpty(queryData.Credentials))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(queryData.Credentials)));
            }

            using var requestMessage = new HttpRequestMessage(queryData.method, queryData.endpoint);
            if (queryData.data != null) 
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(queryData.data), Encoding.UTF8, "application/json");
            

            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
            //return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
            return await response.Content.ReadAsAsync<T>();
        }
    }
}
