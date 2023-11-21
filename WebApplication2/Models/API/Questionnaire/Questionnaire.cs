using WebApplication2.ViewModels;

namespace WebApplication2.Models.API
{
    public class Questionnaire
    {
        public int oid {  get; set; }
        public string name { get; set; }    
        public List<QuestionViewModel> questions { get; set; }   //!!!!?? нужно только когда я беру опр опросник, если беру все, то не нужно || нужно сделать одну модель вопроса
        public int companyOid { get; set; }
        public string company {  get; set; }
    }
}
