namespace WebApplication2.ViewModels
{
    public class CreateQuestionnaire
    {
        public int oid {  get; set; }
        public string name { get; set; }
        public List<CreateQuestion> questions {  get; set; }
        //public int gradingType { get; set; }
        //public string comentary { get; set; }
        //public List<string> answerVariants { get; set; }
        public int companyOid { get; set; }
        public string company {  get; set; }
        public string token { get; set; }

    }
}
