using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
{
    public class GenerateLicenseViewModelVolidator : AbstractValidator<GenerateLicenseViewModel>
    {
        public GenerateLicenseViewModelVolidator()
        {
            RuleFor(x => x.quantity).NotEmpty().InclusiveBetween(1, 100).WithName(Localization.quantity);
        }
    }
}
