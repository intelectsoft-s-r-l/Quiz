using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using WebApplication2.Controllers;
using WebApplication2.Interface;
using WebApplication2.Models.API;
using WebApplication2.Models.API.License;
using WebApplication2.ViewModels;

namespace WebApplication2.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        public async Task<BaseErrors> ActivateLicense(string token, string oid)
        {
            using (var httpClientForActivateLicense = new HttpClient())
            {

                var apiUrlActivateLicense = "https://dev.edi.md/ISNPSAPI/Web/ActivateLicense?token=" + token + "&Oid=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForActivateLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                var responseActivateLicense = await httpClientForActivateLicense.GetAsync(apiUrlActivateLicense);

                if (responseActivateLicense.IsSuccessStatusCode)
                {
                    var licenseBaseResponse = await responseActivateLicense.Content.ReadAsAsync<BaseErrors>();
                    return licenseBaseResponse;
                }
                return new BaseErrors() { errorCode = -1 };
            }
        }

        public async Task<BaseErrors> DeactivateLicense(string token, string oid)
        {
            using (var httpClientForDeactivateLicense = new HttpClient())
            {

                var apiUrlDeactivateLicense = "https://dev.edi.md/ISNPSAPI/Web/DeactivateLicense?token=" + token + "&Oid=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForDeactivateLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                var responseDeactivateLicense = await httpClientForDeactivateLicense.GetAsync(apiUrlDeactivateLicense);

                if (responseDeactivateLicense.IsSuccessStatusCode)
                {
                    var licenseBaseResponse = await responseDeactivateLicense.Content.ReadAsAsync<BaseErrors>();
                    return licenseBaseResponse;

                }
                return new BaseErrors() { errorCode = -1 };
            }
        }

        public async Task<BaseErrors> Delete(string token, string oid)
        {
            using (var httpClientForDeleteLicense = new HttpClient())
            {

                var apiUrlDeleteLicense = "https://dev.edi.md/ISNPSAPI/Web/DeleteLicense?token=" + token + "&Oid=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForDeleteLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseDeleteLicense = await httpClientForDeleteLicense.DeleteAsync(apiUrlDeleteLicense);

                if (responseDeleteLicense.IsSuccessStatusCode)
                {
                    var baseResponsedData = await responseDeleteLicense.Content.ReadAsAsync<BaseErrors>();
                    return baseResponsedData;
                }
                return new BaseErrors() { errorCode = -1 };
            }
        }

        public async Task<BaseErrors> GenerateLicense(GenerateLicenseViewModel generateLicenseVM)
        {
            using (var httpClientForPostLicense = new HttpClient())
            {
                var addingStr = "?token=" + generateLicenseVM.token + "&quantity=" + generateLicenseVM.quantity;

                var apiUrlForPostLicense = "https://dev.edi.md/ISNPSAPI/Web/GenerateLicense" + addingStr;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForPostLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);
                //generateLicenseVM.token = token;

                var jsonContent = new StringContent(JsonConvert.SerializeObject(generateLicenseVM), Encoding.UTF8, "application/json"); //Зачем мне передовать объект

                var responsePostLicense = await httpClientForPostLicense.PostAsync(apiUrlForPostLicense, jsonContent);

                if (responsePostLicense.IsSuccessStatusCode)
                {
                    var baseResponsedData = await responsePostLicense.Content.ReadAsAsync<BaseErrors>();
                    return baseResponsedData;
                    
                }
                return new BaseErrors() { errorCode = -1 };
            }
        }

        public async Task<GetLicense> GetLicense(string token, string oid)
        {
            using (var httpClientForLicense = new HttpClient())
            {

                var apiUrlGetLicense = "https://dev.edi.md/ISNPSAPI/Web/GetLicense?token=" + token + "&Oid=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                var responseGetLicense = await httpClientForLicense.GetAsync(apiUrlGetLicense);

                if (responseGetLicense.IsSuccessStatusCode)
                {
                    var licenseData = await responseGetLicense.Content.ReadAsAsync<GetLicense>();
                    return licenseData;
                }
                return new GetLicense() { errorCode = -1 };
            }
        }

        public async Task<GetLicenseList> GetLicenseList(string token)
        {
            using (var httpClientForLicenseList = new HttpClient())
            {

                var apiUrlGetLicenseListByToken = "https://dev.edi.md/ISNPSAPI/Web/GetLicenseList?token=" + token;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForLicenseList.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);


                var responseGetLicenseList = await httpClientForLicenseList.GetAsync(apiUrlGetLicenseListByToken);

                if (responseGetLicenseList.IsSuccessStatusCode)
                {
                    var licensesData = await responseGetLicenseList.Content.ReadAsAsync<GetLicenseList>();
                    return licensesData;

                }
                return new GetLicenseList() { errorCode = -1 };
            }
        }

        public async Task<BaseErrors> ReleaseLicense(string token, string oid)
        {
            using (var httpClientForReleaseLicense = new HttpClient())
            {

                var apiUrlReleaseLicense = "https://dev.edi.md/ISNPSAPI/Web/ReleaseLicense?token=" + token + "&Oid=" + oid;

                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
                httpClientForReleaseLicense.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials);

                var responseReleaseLicense = await httpClientForReleaseLicense.GetAsync(apiUrlReleaseLicense);

                if (responseReleaseLicense.IsSuccessStatusCode)
                {
                    var licenseBaseResponse = await responseReleaseLicense.Content.ReadAsAsync<BaseErrors>();
                    return licenseBaseResponse;
                }
                return new BaseErrors() { errorCode = -1 };
            }
        }
    }
}
