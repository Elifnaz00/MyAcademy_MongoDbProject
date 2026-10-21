using FluentValidation;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Validations.TourValidations
{
    public class CreateTourValidator : AbstractValidator<CreateTourDto>
    {

        public CreateTourValidator() {

            RuleFor(x => x.Name).NotEmpty().WithMessage("Başlık boş bırakılamaz.").MinimumLength(3).WithMessage("Başlık en az 3 karakter olmalıdır.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.").MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olmalıdır.");

            RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Fiyat bilgisi boş bırakılamaz.");



        }

    }
}
