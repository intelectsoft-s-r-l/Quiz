namespace ISQuiz.Helper
{
    public class GlobalConfiguration
    {
        public string Credentials()
        {
#if (RELEASE)
            return "uSr_nps:V8-}W31S!l'D";
#endif
#if DEBUG
           return "nspapi_usr:4b3pY6<mY)+F";
#endif
        }



        public string StartUriForQuiz()
        {
#if (RELEASE)
            return "https://dev.edi.md/ISNPSAPI/Web/";
#endif
#if DEBUG
           return "https://survey.eservicii.md/ISNPSAPI/Web/";
#endif
        }

    }
}
