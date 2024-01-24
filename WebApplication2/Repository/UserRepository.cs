using ISQuiz.Interface;
using ISQuiz.Models;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;

namespace ISQuiz.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthURLs authURLs = new AuthURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();


        //GET
        public async Task<GetProfileInfo> getProfileInfo(string token)
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

        //POST
        public async Task<BaseResponse> changePassword(ChangePasswordViewModel changePasswordVM)
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


    }

}
