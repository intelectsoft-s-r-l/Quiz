using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class AuthorizeViewModelValidator : AbstractValidator<AuthorizeViewModel>
    {
        public AuthorizeViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName(Localization.emailAddress);
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(4).WithName(Localization.password);
        }
    }
}
