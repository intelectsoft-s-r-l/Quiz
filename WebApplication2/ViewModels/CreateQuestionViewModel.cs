using WebApplication2.Models.Enum;

namespace WebApplication2.ViewModels
{
    public class CreateQuestionViewModel
    {
        public string question { get; set; }
        public GradingType gradingType { get; set; }
        public string comentary { get; set; }
        public List<string> answerVariants { get; set; }
    }
}
