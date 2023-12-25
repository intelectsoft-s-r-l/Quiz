namespace WebApplication2.Models.API.Questionnaires
{
    public class QuestionnaireStatistic
    {
        public int totalResponses { get; set; }
        public int totalFalseResponses { get; set; }
        public int totalTrueResponses { get; set; }
        public int totalGradeLowerOrEqualThan6 { get; set; }
        public int totalGrade7 { get; set; }
        public int totalGrade8 { get; set; }
        public int totalGrade9 { get; set; }
        public int totalGrade10 { get; set; }
    }
}
