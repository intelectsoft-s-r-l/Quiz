using WebApplication2.Models.API;
using WebApplication2.Models.API.License;
using WebApplication2.ViewModels;

namespace WebApplication2.Interface
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
