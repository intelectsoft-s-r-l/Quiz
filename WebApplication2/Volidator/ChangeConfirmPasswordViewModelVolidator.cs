using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
{
    public class ChangeConfirmPasswordViewModelValidator : AbstractValidator<ChangeConfirmPasswordViewModel>
    {
        const string passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[\{\}@?])[A-Za-z\d\{\}@?]{8,}$";

        public ChangeConfirmPasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithName(Localization.OldPassword);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(20)
                .Matches(passwordPattern)
                .WithName(Localization.NewPassword);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword)
                .WithName(Localization.ConfirmPassword);
        }
    }

}
