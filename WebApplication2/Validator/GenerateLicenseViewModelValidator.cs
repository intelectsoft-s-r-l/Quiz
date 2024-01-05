using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class GenerateLicenseViewModelValidator : AbstractValidator<GenerateLicenseViewModel>
    {
        public GenerateLicenseViewModelValidator()
        {
            RuleFor(x => x.quantity).NotEmpty().NotNull().InclusiveBetween(1, 100).WithName(Localization.quantity);
        }
    }
}
