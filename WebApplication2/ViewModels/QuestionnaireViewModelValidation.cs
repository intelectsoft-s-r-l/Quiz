using FluentValidation;

namespace WebApplication2.ViewModels
{
    using FluentValidation;

    public class QuestionnaireViewModelValidation : AbstractValidator<QuestionnaireViewModel>
    {
        public QuestionnaireViewModelValidation()
        {
            RuleFor(x => x.Title).NotNull().MaximumLength(100);

            RuleForEach(x => x.Questions)
                .SetValidator(new QuestionViewModelValidation());
        }
    }

    public class QuestionViewModelValidation : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidation()
        {
            RuleFor(x => x.question).MaximumLength(255);
            RuleFor(x => x.gradingType).NotNull();
            RuleFor(x => x.comentary).MaximumLength(500);

            When(x => x.answerVariants != null, () =>
            {
                RuleFor(x => x.answerVariants).NotEmpty();
            });
        }
    }

}
