using WebApplication2.Models.API;

namespace WebApplication2.ViewModels
{
    public class QuestionnaireViewModel
    {
        public int oid { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

}
