using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Validator
{
    public class GenerateLicenseViewModelVolidator : AbstractValidator<GenerateLicenseViewModel>
    {
        public GenerateLicenseViewModelVolidator()
        {
            RuleFor(x => x.quantity).NotEmpty().NotNull().InclusiveBetween(1, 100).WithName(Localization.quantity);
        }
    }
}
