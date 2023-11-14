using System.Text.RegularExpressions;
using WebApplication2.Models;
using WebApplication2.Models.API;

namespace WebApplication2.Interface
{
    public interface IQuestionnaireRepository
    {
        Task<IList<Questionnaire>> GetAllQuestionnaires(string token);
        IList<Question> GetAllQuestionsByQuestionnaire(int id);
        bool Add(Questionnaire questionnaire);
        bool Update(Questionnaire questionnaire);
        bool Delete(Questionnaire questionnaire);
        bool Save();
    }
}
