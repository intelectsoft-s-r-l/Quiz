using ISQuiz.Models.API;
using ISQuiz.Models.API.Questionnaires;
using ISQuiz.ViewModels;

namespace ISQuiz.Interface
{
    public interface IQuizRepository
    {
        Task<GetQuestionnairesInfo> GetQuestionnaires(string token);
        Task<DetailQuestionnaire> GetQuestionnaire(string token, int id);
        Task<DetailQuestions> GetQuestions(string token, int id);
        Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id);
        Task<QuestionStatistic> GetQuestionStatistic(string token, int id);
        Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM);
        Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM);
        Task<BaseErrors> DeleteQuestionnaire(string token, int oid);
        Task<BaseErrors> DeleteQuestion(string token, int id);
    }
}
