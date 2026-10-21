using FluentValidation;
using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Validations.RouteValidations
{
    public class UpdateRouteValidator : AbstractValidator<UpdateRouteDto>
    {
        public UpdateRouteValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir boş bırakılamaz.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");
            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Tur süresi boş bırakılamaz.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat bilgisi boş bırakılamaz.");
        }
    }
}
