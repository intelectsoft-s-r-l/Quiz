namespace ISQuizBLL.URLs
{
    public class LicenseURLs
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
        public string GetLicenseList(string token) => $"/ISNPSAPI/Web/GetLicenseList?token={token}";
        public string GetLicense(string token, string oid) => $"/ISNPSAPI/Web/GetLicense?token={token}&Oid={oid}";
        public string ActivateLicense(string token, string oid) => $"/ISNPSAPI/Web/ActivateLicense?token={token}&Oid={oid}";
        public string DeactivateLicense(string token, string oid) => $"/ISNPSAPI/Web/DeactivateLicense?token={token}&Oid={oid}";
        public string ReleaseLicense(string token, string oid) => $"/ISNPSAPI/Web/ReleaseLicense?token={token}&Oid={oid}";

        //POST
        public string GenerateLicense(string token, int quantity) => $"/ISNPSAPI/Web/GenerateLicense?token={token}&quantity={quantity}";

        //DELETE
        public string DeleteLicense(string token, string oid) => $"/ISNPSAPI/Web/DeleteLicense?token={token}&Oid={oid}";

    }
}
