namespace WebApplication2.ViewModels
{
    public class CreateQuestionnaire
    {
        public int oid {  get; set; }
        public string name { get; set; }
        public List<CreateQuestionViewModel> questions {  get; set; }
        public int companyOid { get; set; }
        public string company {  get; set; }
        public string token { get; set; }

    }
}
