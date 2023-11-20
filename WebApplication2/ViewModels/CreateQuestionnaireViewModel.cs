using WebApplication2.Models.API;

namespace WebApplication2.ViewModels
{
    public class CreateQuestionnaireViewModel
    {
        public int oid { get; set; }
        public string Title { get; set; }
        public List<CreateQuestionViewModel> Questions { get; set; }
    }

}
