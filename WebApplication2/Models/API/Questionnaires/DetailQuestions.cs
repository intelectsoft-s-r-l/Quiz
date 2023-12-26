using ISQuiz.ViewModels;

namespace ISQuiz.Models.API.Questionnaires
{
    public class DetailQuestions : BaseErrors
    {
        public List<QuestionViewModel> questions { get; set; }
    }
}
