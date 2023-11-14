namespace WebApplication2.Models.API
{
    public class DetailQuestionnaire : BaseErrors
    {
        public Questionnaire questionnaire { get; set; }
        public List<Question> questions { get; set; }
    }
}
