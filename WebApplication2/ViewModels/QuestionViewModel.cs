using ISQuiz.Models.Enum;

namespace ISQuiz.ViewModels
{
    public class QuestionViewModel
    {
        public int id { get; set; }
        public int questionnaireId { get; set; }
        public string question { get; set; }
        public GradingType gradingType { get; set; }
        public string comentary { get; set; }
        public EnQuestionStatus questionStatus { get; set; }
        public int index { get; set; }
        public List<ResponseVariant> responseVariants { get; set; }

    }
}
