using FluentValidation;
using ISQuiz.Resources;
using ISQuiz.ViewModels;

namespace ISQuiz.Validator
{
    public class AuthRecoverpwViewModelVolidator : AbstractValidator<AuthRecoverpwViewModel>
    {
        public AuthRecoverpwViewModelVolidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName(Localization.emailAddress);
        }
    }
}
