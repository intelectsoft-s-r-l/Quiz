using WebApplication2.Models.API;
using WebApplication2.Models.API.Questionnaires;
using WebApplication2.ViewModels;

namespace WebApplication2.Interface
{
    public interface IQuizRepository
    {
        Task<GetQuestionnairesInfo> GetQuestionnaires(string token);
        Task<DetailQuestionnaire> GetQuestionnaire(string token, int id);
        Task<DetailQuestions> GetQuestions(string token, int id);
        Task<QuestionnaireStatisticResponse> GetQuestionnaireStatistic(string token, int id);
        Task<QuestionnaireIdViewModel> UpsertQuestionnaire(UpsertQuestionnaire upsertQuestionnaireVM);
        Task<BaseErrors> UpsertQuestions(UpsertQuestions upsertQuestionsVM);
        Task<BaseErrors> DeleteQuestionnaire(string token, int oid);
        Task<BaseErrors> DeleteQuestion(string token, int id);
    }
}
