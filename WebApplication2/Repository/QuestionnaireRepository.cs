using WebApplication2.Interface;
using WebApplication2.Models.API;

namespace WebApplication2.Repository
{
    public class QuestionnaireRepository : IQuestionnaireRepository
    {
        public bool Add(Questionnaire questionnaire)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Questionnaire questionnaire)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Questionnaire>> GetAllQuestionnaires(string token)
        {
            throw new NotImplementedException();
        }

        public IList<Question> GetAllQuestionsByQuestionnaire(int id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(Questionnaire questionnaire)
        {
            throw new NotImplementedException();
        }
    }
}
