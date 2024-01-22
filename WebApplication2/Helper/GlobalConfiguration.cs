namespace ISQuiz.Helper
{
    public class GlobalConfiguration
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



        public string StartUriForQuiz()
        {
#if (DEBUG)
            return "https://dev.edi.md/ISNPSAPI/Web/";
#endif
#if RELEASE
           return "https://survey.eservicii.md/ISNPSAPI/Web/";
#endif
        }

    }
}
