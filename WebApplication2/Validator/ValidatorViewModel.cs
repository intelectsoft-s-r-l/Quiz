using FluentValidation;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class ValidatorViewModel
    {

        public ValidatorViewModel(IServiceCollection services)
        {
            services.AddTransient<IValidator<AuthRecoverpwViewModel>, AuthRecoverpwViewModelVolidator>();
            services.AddTransient<IValidator<AuthorizeViewModel>, AuthorizeViewModelVolidator>();
            services.AddTransient<IValidator<ChangeConfirmPasswordViewModel>, ChangeConfirmPasswordViewModelValidator>();
            services.AddTransient<IValidator<GenerateLicenseViewModel>, GenerateLicenseViewModelVolidator>();
        }
    }
}
