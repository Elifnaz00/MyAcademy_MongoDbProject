using FluentValidation;
using FluentValidation.AspNetCore;
using Travel.Web.DTOs.AuthDtos;

namespace Travel.Web.Validations.UserValidations
{
    public class CreateUserValidator : AbstractValidator<RegisterDto>
    {
        public CreateUserValidator() {
            RuleFor(x => x.ConfirmPassword)
                .Equal(x=>x.Password).WithMessage("Şifreler aynı olmalıdır.");



        }
    }
}
