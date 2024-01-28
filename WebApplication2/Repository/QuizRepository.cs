using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;
using Serilog;

namespace ISQuiz.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizURLs quizURLs = new QuizURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();

        //GET
        public async Task<DetailQuestionnaire> GetQuestionnaire(string token, int id)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new DetailQuestionnaire
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }


        public async Task<GetQuestionnairesInfo> GetQuestionnaires(string token)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new GetQuestionnairesInfo
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }


        public async Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new QuestionnaireStatisticResponse
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }
        }


        public async Task<DetailQuestions> GetQuestions(string token, int id)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new DetailQuestions
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }


        //POST(Upsert)
        public async Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new QuestionnaireIdViewModel
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }


        public async Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        //DELETE
        public async Task<BaseErrors> DeleteQuestion(string token, int id)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }
        }

        public async Task<BaseErrors> DeleteQuestionnaire(string token, int oid)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }
        }
    }

}
