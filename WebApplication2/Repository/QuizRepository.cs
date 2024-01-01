using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace ISQuiz.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly HttpClient _httpClient;

        public QuizRepository()
        {
            _httpClient = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
            _httpClient.BaseAddress = new Uri("https://dev.edi.md/ISNPSAPI/Web/");
        }

        private async Task<T> SendRequest<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        public async Task<BaseErrors> DeleteQuestion(string token, int id)
            => await SendRequest<BaseErrors>($"DeleteQuestions?token={token}&questionId={id}");

        public async Task<BaseErrors> DeleteQuestionnaire(string token, int oid)
            => await SendRequest<BaseErrors>($"DeleteQuestionnaire?Token={token}&id={oid}");

        public async Task<DetailQuestionnaire> GetQuestionnaire(string token, int id)
            => await SendRequest<DetailQuestionnaire>($"GetQuestionnaire?Token={token}&id={id}");

        public async Task<GetQuestionnairesInfo> GetQuestionnaires(string token)
            => await SendRequest<GetQuestionnairesInfo>($"GetQuestionnaires?token={token}");

        public async Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id)
            => await SendRequest<QuestionnaireStatisticResponse>($"QuestionnaireStatistic?token={token}&id={id}");

        public async Task<DetailQuestions> GetQuestions(string token, int id)
            => await SendRequest<DetailQuestions>($"GetQuestions?token={token}&questionnaireId={id}");

        private async Task<T> SendPostRequest<T>(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, jsonContent);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        public async Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM)
            => await SendPostRequest<QuestionnaireIdViewModel>("UpsertQuestionnaire", upsertQuestionnaireVM);

        public async Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM)
            => await SendPostRequest<BaseErrors>("UpsertQuestions", upsertQuestionsVM);
    }

}
