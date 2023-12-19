namespace WebApplication2.ViewModels
{
    public class UpsertQuestionsViewModel
    {
        public int id { get; set; }
        public int questionnaireId { get; set; }
        public string question { get; set; }
        public int gradingType { get; set; }
        public string comentary { get; set; }
        public string responseVariant { get; set; } //Json
    }
}
