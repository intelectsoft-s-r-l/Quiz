using Newtonsoft.Json;
using System.Text;
using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.License;
using ISQuiz.ViewModels;
using System.Net.Http.Headers;
using ISQuiz.Models.API.Questionnaires;

namespace ISQuiz.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly HttpClient _httpClient;
        public LicenseRepository()
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("uSr_nps:V8-}W31S!l'D"));
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://dev.edi.md/ISNPSAPI/Web/"),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", credentials)
                }
            };
        }

        private async Task<T> SendRequest<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        private async Task<T> SendPostRequest<T>(string endpoint, object data)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, jsonContent);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }
        private async Task<T> SendDeleteRequest<T>(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsAsync<T>() : default;
        }

        //GET
        public async Task<BaseErrors> ActivateLicense(string token, string oid)
            => await SendRequest<BaseErrors>($"ActivateLicense?token={token}&Oid={oid}");

        public async Task<BaseErrors> DeactivateLicense(string token, string oid)
            => await SendRequest<BaseErrors>($"DeactivateLicense?token={token}&Oid={oid}");

        public async Task<BaseErrors> ReleaseLicense(string token, string oid)
            => await SendRequest<BaseErrors>($"ReleaseLicense?token={token}&Oid={oid}");

        public async Task<GetLicense> GetLicense(string token, string oid)
             => await SendRequest<GetLicense>($"GetLicense?token={token}&Oid={oid}");

        public async Task<GetLicenseList> GetLicenseList(string token)
            => await SendRequest<GetLicenseList>($"GetLicenseList?token={token}");


        //POST
        public async Task<BaseErrors> GenerateLicense(GenerateLicenseViewModel generateLicenseVM)
            => await SendPostRequest<BaseErrors>($"GenerateLicense?token={generateLicenseVM.token}&quantity={generateLicenseVM.quantity}", generateLicenseVM);

        //DELETE
        public async Task<BaseErrors> Delete(string token, string oid)
            => await SendDeleteRequest<BaseErrors>($"DeleteLicense?token={token}&Oid={oid}");
    }
}
