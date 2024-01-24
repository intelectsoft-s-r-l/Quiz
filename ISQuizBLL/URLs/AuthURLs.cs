namespace ISQuizBLL.URLs
{
    public class AuthURLs
    {
        public string Credentials()
        {
#if (DEBUG)
            return "";
#endif
#if RELEASE
            return "";
#endif
        }


        //POST
        public string AuthorizeUser() =>"/ISAuthService/json/AuthorizeUser";
        

        //POST
        public string ChangePassword() => "/ISAuthService/json/ChangePassword";
        

        //GET
        public string ChangeUILanguage(string Token, int Language) => "/ISAuthService/json/ChangeUILanguage?Token=" + Token + "&Language=" + Language;  


        //GET
        public string GetProfileInfo(string Token) => "/ISAuthService/json/GetProfileInfo?Token=" + Token;
        

        //GET
        public string RefreshToken(string Token) => "/ISAuthService/json/RefreshToken?Token=" + Token;
        

        //POST
        public string ResetPassword() => "/ISAuthService/json/ResetPassword";
        
    }
}
