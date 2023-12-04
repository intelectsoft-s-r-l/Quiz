using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
{
    public class AuthRecoverpwViewModelVolidator : AbstractValidator<AuthRecoverpwViewModel>
    {
        public AuthRecoverpwViewModelVolidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName(Localization.emailAddress);
        }
    }
}
