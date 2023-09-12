using AuthServer.Core.DTOs;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserDtoValidator:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is Required").EmailAddress().WithMessage("Not Valid Email");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is Required");
        }
    }
}
