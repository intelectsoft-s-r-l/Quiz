using WebApplication2.ViewModels;

namespace WebApplication2.Models.API.Questionnaires
{
    public class Questionnaire
    {
        public int oid {  get; set; }
        public string name { get; set; }    
        public int companyOid { get; set; }
        public string company {  get; set; }
    }
}
