using WebApplication2.Models.API;

namespace WebApplication2.ViewModels
{
    public class CreateQuestionnaireViewModel
    {
        public string Title { get; set; }
        public List<CreateQuestionViewModel> Questions { get; set; }
    }

}
