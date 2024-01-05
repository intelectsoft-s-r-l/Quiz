using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class AuthRecoverpwViewModelValidator : AbstractValidator<AuthRecoverpwViewModel>
    {
        public AuthRecoverpwViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName(Localization.emailAddress);
        }
    }
}
