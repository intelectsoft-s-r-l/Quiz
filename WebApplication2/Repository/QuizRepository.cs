using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;

namespace ISQuiz.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizURLs quizURLs = new QuizURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();

        //GET
        public async Task<DetailQuestionnaire> GetQuestionnaire(string token, int id)
        {
            var url = quizURLs.GetQuestionnaire(token, id);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Get,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<DetailQuestionnaire>(queryData);
        }


        public async Task<GetQuestionnairesInfo> GetQuestionnaires(string token)
        {
            var url = quizURLs.GetQuestionnaires(token);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Get,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<GetQuestionnairesInfo>(queryData);
        }


        public async Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id)
        {
            var url = quizURLs.QuestionnaireStatistic(token, id);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Get,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<QuestionnaireStatisticResponse>(queryData);
        }


        public async Task<DetailQuestions> GetQuestions(string token, int id)
        {
            var url = quizURLs.GetQuestions(token, id);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Get,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<DetailQuestions>(queryData);
        }


        //POST(Upsert)
        public async Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM)
        {
            var url = quizURLs.UpsertQuestionnaire();
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Post,
                endpoint = url,
                Credentials = credentials,
                data = upsertQuestionnaireVM
            };
            return await GlobalQuery.SendRequest<QuestionnaireIdViewModel>(queryData);
        }


        public async Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM)
        {
            var url = quizURLs.UpsertQuestions();
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Post,
                endpoint = url,
                Credentials = credentials,
                data = upsertQuestionsVM
            };
            return await GlobalQuery.SendRequest<BaseErrors>(queryData);
        }

        //DELETE
        public async Task<BaseErrors> DeleteQuestion(string token, int id)
        {
            var url = quizURLs.DeleteQuestions(token, id);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Delete,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<BaseErrors>(queryData);
        }

        public async Task<BaseErrors> DeleteQuestionnaire(string token, int oid)
        {
            var url = quizURLs.DeleteQuestionnaire(token, oid);
            var credentials = quizURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Delete,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<BaseErrors>(queryData);
        }
    }

}
