using FluentValidation;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.QıuestionDtos;

namespace Travel.Web.Validations.QuestionValidations
{
    public class ReplyQuestionValidator : AbstractValidator<ReplyQuestionDto>
    {
        public ReplyQuestionValidator()
        {
            
            RuleFor(x => x.AnsweredText).NotEmpty().WithMessage("Cevap alanı boş bırakılamaz.").MaximumLength(250).WithMessage("Cevap alanı en fazla 250 karakter olmalıdır.");
        }
    }
}
