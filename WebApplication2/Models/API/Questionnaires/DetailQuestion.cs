using ISQuiz.ViewModels;

namespace ISQuiz.Models.API.Questionnaires
{
    public class DetailQuestion : BaseErrors
    {
        public QuestionViewModel question { get; set; }
    }
}
