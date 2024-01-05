using FluentValidation;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class ValidatorViewModel
    {

        public ValidatorViewModel(IServiceCollection services)
        {
            services.AddTransient<IValidator<AuthRecoverpwViewModel>, AuthRecoverpwViewModelValidator>();
            services.AddTransient<IValidator<AuthorizeViewModel>, AuthorizeViewModelValidator>();
            services.AddTransient<IValidator<ChangeConfirmPasswordViewModel>, ChangeConfirmPasswordViewModelValidator>();
            services.AddTransient<IValidator<GenerateLicenseViewModel>, GenerateLicenseViewModelValidator>();
        }
    }
}
