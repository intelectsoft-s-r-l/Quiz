using ISQuiz.Interface;
using ISQuiz.Models.API;
using ISQuiz.Models.API.License;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;
using Serilog;

namespace ISQuiz.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly LicenseURLs licenseURLs = new LicenseURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();


        //GET
        public async Task<BaseErrors> ActivateLicense(string token, string oid)
        {
            try
            {
                var url = licenseURLs.ActivateLicense(token, oid);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<BaseErrors>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        public async Task<BaseErrors> DeactivateLicense(string token, string oid)
        {
            try
            {
                var url = licenseURLs.DeactivateLicense(token, oid);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<BaseErrors>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        public async Task<BaseErrors> ReleaseLicense(string token, string oid)
        {
            try
            {
                var url = licenseURLs.ReleaseLicense(token, oid);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<BaseErrors>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        public async Task<GetLicense> GetLicense(string token, string oid)
        {
            try
            {
                var url = licenseURLs.GetLicense(token, oid);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<GetLicense>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new GetLicense
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        public async Task<GetLicenseList> GetLicenseList(string token)
        {
            try
            {
                var url = licenseURLs.GetLicenseList(token);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<GetLicenseList>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new GetLicenseList
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }



        //POST
        public async Task<BaseErrors> GenerateLicense(GenerateLicenseViewModel generateLicenseVM)
        {
            try
            {
                var url = licenseURLs.GenerateLicense(generateLicenseVM.token, generateLicenseVM.quantity);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Post,
                    endpoint = url,
                    Credentials = credentials,
                    data = generateLicenseVM
                };
                return await GlobalQuery.SendRequest<BaseErrors>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }


        //DELETE
        public async Task<BaseErrors> Delete(string token, string oid)
        {
            try
            {
                var url = licenseURLs.DeleteLicense(token, oid);
                var credentials = licenseURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Delete,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<BaseErrors>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseErrors
                {
                    errorCode = EnErrorCode.Internal_error,
                    errorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

    }
}


