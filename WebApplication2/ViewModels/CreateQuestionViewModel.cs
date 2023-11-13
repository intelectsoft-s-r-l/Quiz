using WebApplication2.Models.Enum;

namespace WebApplication2.ViewModels
{
    //public class CreateQuestionViewModel
    //{
    //    public string Text { get; set; }
    //    public GradingType gradingType { get; set; }
    //    public string? comentary { get; set; }
    //    public List<string> Answers { get; set; } // ListAnsw and null
    //}
    public class CreateQuestionViewModel
    {
        public string Text { get; set; }
        public GradingType gradingType { get; set; }
        public string comentary { get; set; }
        public List<string> Answers { get; set; }
    }
}
