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

        //GET
        public string ActivateUser(string Token)
        {
            return "/ISAuthService/json/ActivateUser?Token=" + Token;
        }

        //POST
        public string AuthorizeUser()
        {
            return "/ISAuthService/json/AuthorizeUser";
        }

        //POST
        public string ChangePassword()
        {
            return "/ISAuthService/json/ChangePassword";
        }

        //GET
        public string ChangeUILanguage(string Token, int Language)
        {
            return "/ISAuthService/json/ChangeUILanguage?Token=" + Token + "&Language=" + Language;
        }

        //GET
        public string GetApiKey(string CompanyID, string Product, string ProductModule, string ProductCategory)
        {
            return "/ISAuthService/json/GetApiKey?CompanyID=" + CompanyID + "&Product=" + Product + "&ProductModule=" + ProductModule + "&ProductCategory=" + ProductCategory;
        }

        //GET
        public string GetApiKeyInfo(string ApiKey)
        {
            return "/ISAuthService/json/GetApiKeyInfo?ApiKey=" + ApiKey;
        }

        //GET
        public string GetCompanyID(string ApiKey, string IDNO)
        {
            return "/ISAuthService/json/GetCompanyID?ApiKey=" + ApiKey + "&IDNO=" + IDNO;
        }

        //GET
        public string GetCompanyInfo(string Token, Guid ID)
        {
            return "/ISAuthService/json/GetCompanyInfo?Token=" + Token + "&ID=" + ID;
        }

        //GET
        public string GetInfo()
        {
            return "/ISAuthService/json/GetInfo";
        }

        //GET
        public string GetLicenseInfo(Guid ID)
        {
            return "/ISAuthService/json/GetLicenseInfo?ID=" + ID;
        }

        //GET
        public string GetManagedToken(string Token, int CompanyID)
        {
            return "/ISAuthService/json/GetManagedToken?Token=" + Token + "&CompanyID=" + CompanyID;
        }

        //GET
        public string GetProfileInfo(string Token)
        {
            return "/ISAuthService/json/GetProfileInfo?Token=" + Token;
        }

        //GET
        public string RefreshToken(string Token)
        {
            return "/ISAuthService/json/RefreshToken?Token=" + Token;
        }

        //POST
        public string ResetPassword()
        {
            return "/ISAuthService/json/ResetPassword";
        }
    }
}
