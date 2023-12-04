using FluentValidation;
using WebApplication2.Resources;
using WebApplication2.ViewModels;

namespace WebApplication2.Volidator
{
    public class ChangeConfirmPasswordViewModelValidator : AbstractValidator<ChangeConfirmPasswordViewModel>
    {
        public ChangeConfirmPasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithName(Localization.OldPassword);//.WithMessage("Пожалуйста, введите старый пароль.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()/*.WithMessage("Пожалуйста, введите новый пароль.")*/.MinimumLength(8).MaximumLength(20).WithName(Localization.NewPassword);//.WithMessage("Новый пароль должен содержать как минимум 6 символов.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()/*.WithMessage("Пожалуйста, подтвердите новый пароль.")*/
                .Equal(x => x.NewPassword).WithName(Localization.ConfirmPassword);//.WithMessage("Подтверждение пароля не совпадает с новым паролем.");
        }
    }

}
