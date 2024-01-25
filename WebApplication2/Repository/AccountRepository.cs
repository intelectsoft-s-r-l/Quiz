using ISQuiz.Interface;
using ISQuiz.Models;
using ISQuiz.Models.Enum;
using ISQuiz.ViewModels;
using ISQuizBLL.Queries;
using ISQuizBLL.URLs;

namespace ISQuiz.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AuthURLs authURLs = new AuthURLs();
        private readonly GlobalQuery GlobalQuery = new GlobalQuery();

        // GET
        public async Task<BaseResponse> ChangeUILanguage(string token, EnUiLanguage uiLanguage)
        {
            var url = authURLs.ChangeUILanguage(token, (int)uiLanguage);
            var credentials = authURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Get,
                endpoint = url,
                Credentials = credentials,
            };
            return await GlobalQuery.SendRequest<BaseResponse>(queryData);
        }

        // POST
        public async Task<GetProfileInfo> AuthorizeUser(AuthorizeViewModel loginVM)
        {
            var url = authURLs.AuthorizeUser();
            var credentials = authURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Post,
                endpoint = url,
                Credentials = credentials,
                data = loginVM
            };
            return await GlobalQuery.SendRequest<GetProfileInfo>(queryData);
        }

        public async Task<BaseResponse> RecoverPassword(AuthRecoverpwViewModel recoverpwVM)
        {
            var url = authURLs.ResetPassword();
            var credentials = authURLs.Credentials();

            QueryData queryData = new QueryData()
            {
                method = HttpMethod.Post,
                endpoint = url,
                Credentials = credentials,
                data = recoverpwVM
            };
            return await GlobalQuery.SendRequest<BaseResponse>(queryData);
        }

    }

}
