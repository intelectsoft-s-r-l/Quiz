using ISQuizBLL.Queries;
using Newtonsoft.Json;
using Serilog;
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
            try
            {
                if (queryData.endpoint.Contains("ISNPSAPI"))
                {
#if DEBUG
                    BaseURI = "https://dev.edi.md";
#elif RELEASE
            BaseURI = "https://survey.eservicii.md";
#endif
                }

                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(BaseURI + queryData.endpoint),
                };

                if (!string.IsNullOrEmpty(queryData.Credentials))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(queryData.Credentials)));
                }

                using var requestMessage = new HttpRequestMessage(queryData.method, queryData.endpoint);
                if (queryData.data != null)
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(queryData.data), Encoding.UTF8, "application/json");

                using var response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode(); // This will throw an exception if the response status code is not success.

                return await response.Content.ReadAsAsync<T>();


            }
            catch (HttpRequestException ex)
            {

                Log.Error(ex, $"HttpRequestException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions, log, or rethrow as needed.
                Log.Error(ex, ex.Message);
                throw;
            }
        }

    }
}
