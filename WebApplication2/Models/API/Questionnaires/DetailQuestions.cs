using WebApplication2.ViewModels;

namespace WebApplication2.Models.API.Questionnaires
{
    public class DetailQuestions : BaseErrors
    {
        public List<QuestionViewModel> questions { get; set; }
    }
}
