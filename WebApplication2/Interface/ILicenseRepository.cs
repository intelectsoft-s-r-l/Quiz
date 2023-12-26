using ISQuiz.Models.API;
using ISQuiz.Models.API.License;
using ISQuiz.ViewModels;

namespace ISQuiz.Interface
{
    public interface ILicenseRepository
    {
        Task<GetLicenseList> GetLicenseList(string token);
        Task<GetLicense> GetLicense(string token, string oid);
        Task<BaseErrors> DeactivateLicense(string token, string oid);
        Task<BaseErrors> ActivateLicense(string token, string oid);
        Task<BaseErrors> ReleaseLicense(string token, string oid);
        Task<BaseErrors> GenerateLicense(GenerateLicenseViewModel generateLicenseVM);
        Task<BaseErrors> Delete(string token, string oid);
    }
}
