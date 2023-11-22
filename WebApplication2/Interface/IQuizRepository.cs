using WebApplication2.Models.API;
using WebApplication2.ViewModels;

namespace WebApplication2.Interface
{
    public interface IQuizRepository
    {
        Task<GetQuestionnaireInfo> GetQuestionnaires(string token);
        Task<DetailQuestionnaire> GetQuestionnaire(string token, int id);
        Task<BaseErrors> Upsert(UpsertQuestionnaire upsertQuestionnaireVM);
        Task<BaseErrors> Delete(string token, int oid);
    }
}
