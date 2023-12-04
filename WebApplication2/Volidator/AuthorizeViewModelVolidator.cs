using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
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
