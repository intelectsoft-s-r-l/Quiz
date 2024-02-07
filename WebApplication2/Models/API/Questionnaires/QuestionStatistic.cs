namespace ISQuiz.Models.API.Questionnaires
{
    public class QuestionStatistic : BaseErrors
    {
        public yesNoStatistic yesNoStatistic { get; set; }
        public point10ScoreStatistic point10ScoreStatistic { get; set; }
        public singleAnswerVariantStatistic singleAnswerVariantStatistic { get; set; }
        public multipleAnswerVariantStatistic multipleAnswerVariantStatistic { get; set; }
        public point5ScoreStatistic point5ScoreStatistic { get; set; }
    }

    public class yesNoStatistic
    {
        public int totalResponses { get; set; }
        public int totalYes { get; set; }
        public int totalNo { get; set; }
    }

    public class point10ScoreStatistic
    {
        public int totalResponses { get; set; }
        public int totalGradeLowerOrEqualThan6 { get; set; }
        public int totalGrade7 { get; set; }
        public int totalGrade8 { get; set; }
        public int totalGrade9 { get; set; }
        public int totalGrade10 { get; set; }
    }

    public class singleAnswerVariantStatistic
    {
        public int totalResponses { get; set; }
    }
    
    public class multipleAnswerVariantStatistic
    {
        public int totalResponses { get; set; }
    }

    public class point5ScoreStatistic
    {
        public int totalResponses { get; set; }
        public int totalGrade1 { get; set; }
        public int totalGrade2 { get; set; }
        public int totalGrade3 { get; set; }
        public int totalGrade4 { get; set; }
        public int totalGrade5 { get; set; }
    }
}
