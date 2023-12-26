using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class GenerateLicenseViewModelVolidator : AbstractValidator<GenerateLicenseViewModel>
    {
        public GenerateLicenseViewModelVolidator()
        {
            RuleFor(x => x.quantity).NotEmpty().NotNull().InclusiveBetween(1, 100).WithName(Localization.quantity);
        }
    }
}
