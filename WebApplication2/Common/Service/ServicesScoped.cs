using ISQuiz.Interface;
using ISQuiz.Models.Interfaces;
using ISQuiz.Repository;

namespace ISQuiz.Common.Service
{
    public class ServicesScoped
    {
        public ServicesScoped(IServiceCollection services)
        {
            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
        }
    }
}
