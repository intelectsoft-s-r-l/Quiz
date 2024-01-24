namespace ISQuizBLL.URLs
{
    public class QuizURLs
    {
        public string Credentials()
        {
#if (DEBUG)
            return "uSr_nps:V8-}W31S!l'D";
#endif
#if RELEASE
           return "nspapi_usr:4b3pY6<mY)+F";
#endif
        }

        //GET
        public string GetQuestionnaires(string token) => $"/ISNPSAPI/Web/GetQuestionnaires?token={token}";
        public string GetQuestionnaire(string token, int id) => $"/ISNPSAPI/Web/GetQuestionnaire?Token={token}&id={id}";
        public string GetQuestions(string token, int questionnaireId) => $"/ISNPSAPI/Web/GetQuestions?token={token}&questionnaireId={questionnaireId}";
        public string GetQuestion(string token, int questionId) => $"/ISNPSAPI/Web/GetQuestion?token={token}&questionId={questionId}";
        public string QuestionnaireStatistic(string token, int id) => $"/ISNPSAPI/Web/QuestionnaireStatistic?token={token}&id={id}";

        //POST
        public string UpsertQuestionnaire() => $"/ISNPSAPI/Web/UpsertQuestionnaire";
        public string UpsertQuestions() => $"/ISNPSAPI/Web/UpsertQuestions";

        //DELETE
        public string DeleteQuestionnaire(string token, int id) => $"/ISNPSAPI/Web/DeleteQuestionnaire?Token={token}&id={id}";
        public string DeleteQuestions(string token, int questionId) => $"/ISNPSAPI/Web/DeleteQuestions?Token={token}&questionId={questionId}";
    }
}
