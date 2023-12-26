using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class AuthorizeViewModelVolidator : AbstractValidator<AuthorizeViewModel>
    {
        public AuthorizeViewModelVolidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName(Localization.emailAddress);
            RuleFor(x => x.Password).NotEmpty().NotNull().MinimumLength(4).WithName(Localization.password);
        }
    }
}
