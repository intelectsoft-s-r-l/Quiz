using FluentValidation;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
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
