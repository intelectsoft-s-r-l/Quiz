using Newtonsoft.Json;
using System.Text;
using WebApplication2.Interface;
using WebApplication2.Models.API;
using WebApplication2.Models.API.Questionnaires;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public class QuizRepository : IQuizRepository
    {
        public async Task<BaseErrors> Delete(string token, int oid)
        {
            using (var httpClientForDeleteQuestionnaire = new HttpClient())
            {

                var apiUrlDeleteQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/DeleteQuestionnaire?Token=" + token + "&id=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForDeleteQuestionnaire.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetQuestionnaires = await httpClientForDeleteQuestionnaire.DeleteAsync(apiUrlDeleteQuestionnaire);

                if (responseGetQuestionnaires.IsSuccessStatusCode)
                {
                    return await responseGetQuestionnaires.Content.ReadAsAsync<BaseErrors>();

                }
                return new BaseErrors() { errorCode = -1 };
            }
        }

        public async Task<DetailQuestionnaire> GetQuestionnaire(string token, int id)
        {
            using (var httpClientForQuestionnaire = new HttpClient())
            {

                var apiUrlGetQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaire?Token=" + token + "&id=" + id;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForQuestionnaire.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetQuestionnaire = await httpClientForQuestionnaire.GetAsync(apiUrlGetQuestionnaire);

                if (responseGetQuestionnaire.IsSuccessStatusCode)
                {
                    var questionnaireData = await responseGetQuestionnaire.Content.ReadAsAsync<DetailQuestionnaire>();
                    return questionnaireData;
                }
                return new DetailQuestionnaire() { errorCode = -1 };
            }
        }

        public async Task<GetQuestionnairesInfo> GetQuestionnaires(string token)
        {
            using (var httpClientForQuestionnaires = new HttpClient())
            {

                var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/GetQuestionnaires?token=" + token;
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));

                // Добавляем аутентификацию в заголовок Authorization с префиксом "Basic ".
                httpClientForQuestionnaires.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetQuestionnaires = await httpClientForQuestionnaires.GetAsync(apiUrlGetQuestionnairesByToken);

                if (responseGetQuestionnaires.IsSuccessStatusCode)
                {
                    var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<GetQuestionnairesInfo>();
                    return questionnaireData;
                }
                return new GetQuestionnairesInfo { errorCode = -1 };
            }
        }

        public async Task<QuestionnaireResponse> GetResponse(string token, int id)
        {
            using (var httpClientForQuestionnaires = new HttpClient())
            {

                var apiUrlGetQuestionnairesByToken = "https://dev.edi.md/ISNPSAPI/Web/QuestionnaireResponse?token=" + token + "&id=" + id;
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForQuestionnaires.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetQuestionnaires = await httpClientForQuestionnaires.GetAsync(apiUrlGetQuestionnairesByToken);

                if (responseGetQuestionnaires.IsSuccessStatusCode)
                {
                    var questionnaireData = await responseGetQuestionnaires.Content.ReadAsAsync<QuestionnaireResponse>();
                    return questionnaireData;
                }
                return new QuestionnaireResponse { errorCode = -1 };
            }
        }

        public async Task<QuestionnaireResponses> GetResponses(string token, int id)
        {
            using (var httpClientForQuestionnaire = new HttpClient())
            {

                var apiUrlGetQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/QuestionnaireResponses?token=" + token + "&questionaireId=" + id;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForQuestionnaire.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetQuestionnaire = await httpClientForQuestionnaire.GetAsync(apiUrlGetQuestionnaire);

                if (responseGetQuestionnaire.IsSuccessStatusCode)
                {
                    return await responseGetQuestionnaire.Content.ReadAsAsync<QuestionnaireResponses>();
                    // return questionnaireData;
                }
                return new QuestionnaireResponses() { errorCode = -1 };
            }
        }



        public async Task<BaseErrors> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM)
        {
            using (var httpClientForEditQuestionnaire = new HttpClient())
            {
                var apiUrlForEditQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/UpsertQuestionnaire";

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForEditQuestionnaire.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var jsonContent = new StringContent(JsonConvert.SerializeObject(upsertQuestionnaireVM), Encoding.UTF8, "application/json");

                var responseEditQuestionnaire = await httpClientForEditQuestionnaire.PostAsync(apiUrlForEditQuestionnaire, jsonContent);

                if (responseEditQuestionnaire.IsSuccessStatusCode)
                {
                    var questionnaireBaseResponsedData = await responseEditQuestionnaire.Content.ReadAsAsync<BaseErrors>();
                    return questionnaireBaseResponsedData;
                }
                return new BaseErrors { errorCode = -1 };
            }
        }

        public async Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM)
        {
            using (var httpClientForEditQuestionnaire = new HttpClient())
            {
                var apiUrlForEditQuestionnaire = "https://dev.edi.md/ISNPSAPI/Web/UpsertQuestions";

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForEditQuestionnaire.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var jsonContent = new StringContent(JsonConvert.SerializeObject(upsertQuestionsVM), Encoding.UTF8, "application/json");

                var responseEditQuestionnaire = await httpClientForEditQuestionnaire.PostAsync(apiUrlForEditQuestionnaire, jsonContent);

                if (responseEditQuestionnaire.IsSuccessStatusCode)
                {
                    var questionnaireBaseResponsedData = await responseEditQuestionnaire.Content.ReadAsAsync<BaseErrors>();
                    return questionnaireBaseResponsedData;
                }
                return new BaseErrors { errorCode = -1 };
            }
        }
    }
}
