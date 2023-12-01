using FluentValidation;
using WebApplication2.ViewModels;

namespace ISAdminWeb.Models
{
    public class ValidatorViewModel
    {

        public ValidatorViewModel(IServiceCollection services)
        {
            services.AddTransient<IValidator<QuestionnaireViewModel>, QuestionnaireViewModelValidation>();
            services.AddTransient<IValidator<QuestionViewModel>, QuestionViewModelValidation>();
        }
    }
}
