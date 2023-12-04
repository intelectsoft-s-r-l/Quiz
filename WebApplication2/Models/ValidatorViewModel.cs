using FluentValidation;
using WebApplication2.ViewModels;
using WebApplication2.Volidator;

namespace ISAdminWeb.Models
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
