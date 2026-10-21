using FluentValidation;
using Travel.Web.DTOs.CommentDtos;

namespace Travel.Web.Validations.CommentValidations
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.CommentContent).NotEmpty().WithMessage("Yorum alanı boş bırakılamaz.").MaximumLength(250).WithMessage("Yorum alanı en fazla 250 karakter olmalıdır.");
        }
    }
}
