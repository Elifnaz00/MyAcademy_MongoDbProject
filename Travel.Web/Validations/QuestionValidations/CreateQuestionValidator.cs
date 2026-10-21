using FluentValidation;
using Travel.Web.DTOs.QıuestionDtos;

namespace Travel.Web.Validations.QuestionValidations
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionDto>
    {
        public CreateQuestionValidator()
        {
            RuleFor(x => x.QuestionContent).NotEmpty().WithMessage("Soru alanı boş bırakılamaz.").MaximumLength(250).WithMessage("Soru alanı en fazla 250 karakter olmalıdır.");
        }
    }
}
