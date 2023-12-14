using WebApplication2.Models.Enum;

namespace WebApplication2.ViewModels
{
    public class QuestionViewModel
    {
        public int id { get; set; }
        public int questionnaireId { get; set; }
        public string question { get; set; }
        public GradingType gradingType { get; set; }
        public string comentary { get; set; }
       public List<ResponseVariant> responseVariants { get; set; }

    }
}
