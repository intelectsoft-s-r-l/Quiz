using ISQuiz.Interface;
using ISQuiz.Models;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;
using Serilog;

namespace ISQuiz.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthURLs authURLs = new AuthURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();


        //GET
        public async Task<GetProfileInfo> getProfileInfo(string token)
        {
            try
            {
                var url = authURLs.GetProfileInfo(token);
                var credentials = authURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Get,
                    endpoint = url,
                    Credentials = credentials,
                };
                return await GlobalQuery.SendRequest<GetProfileInfo>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new GetProfileInfo
                {
                    ErrorCode = EnErrorCode.Internal_error,
                    ErrorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }

        }

        //POST
        public async Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM)
        {
            try
            {
                var url = authURLs.ChangePassword();
                var credentials = authURLs.Credentials();

                QueryData queryData = new QueryData()
                {
                    method = HttpMethod.Post,
                    endpoint = url,
                    Credentials = credentials,
                    data = changePasswordVM
                };
                return await GlobalQuery.SendRequest<BaseResponse>(queryData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                return new BaseResponse
                {
                    ErrorCode = EnErrorCode.Internal_error,
                    ErrorMessage = ex.Message + "|||" + ex.StackTrace,
                };
            }
        }


    }

}
