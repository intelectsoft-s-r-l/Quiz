using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ISQuiz.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly HttpClient _httpClient;

        public QuizRepository(/*IConfiguration configuration*/)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://dev.edi.md/ISNPSAPI/Web/"),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", credentials)
                }
            };
            //_httpClient.BaseAddress = new Uri("https://dev.edi.md/ISNPSAPI/Web/");
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
        }

        private async Task<T> SendGetRequest<T>(string endpoint)
        {
            using var response = await _httpClient.GetAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }
        private async Task<T> SendPostRequest<T>(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(endpoint, jsonContent);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }
        private async Task<T> SendDeleteRequest<T>(string endpoint)
        {
            using var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        //GET
        public async Task<DetailQuestionnaire> GetQuestionnaire(string token, int id)
            => await SendGetRequest<DetailQuestionnaire>($"GetQuestionnaire?Token={token}&id={id}");

        public async Task<GetQuestionnairesInfo> GetQuestionnaires(string token)
            => await SendGetRequest<GetQuestionnairesInfo>($"GetQuestionnaires?token={token}");

        public async Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id)
            => await SendGetRequest<QuestionnaireStatisticResponse>($"QuestionnaireStatistic?token={token}&id={id}");

        public async Task<DetailQuestions> GetQuestions(string token, int id)
            => await SendGetRequest<DetailQuestions>($"GetQuestions?token={token}&questionnaireId={id}");

        //POST(Upsert)
        public async Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM)
            => await SendPostRequest<QuestionnaireIdViewModel>("UpsertQuestionnaire", upsertQuestionnaireVM);

        public async Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM)
            => await SendPostRequest<BaseErrors>("UpsertQuestions", upsertQuestionsVM);

        //DELETE
        public async Task<BaseErrors> DeleteQuestion(string token, int id)
            => await SendDeleteRequest<BaseErrors>($"DeleteQuestions?token={token}&questionId={id}");

        public async Task<BaseErrors> DeleteQuestionnaire(string token, int oid)
            => await SendDeleteRequest<BaseErrors>($"DeleteQuestionnaire?Token={token}&id={oid}");
    }

}
