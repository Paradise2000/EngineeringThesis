using EngineeringThesisAPI.Entities;
using FluentValidation;

namespace EngineeringThesisAPI.DTOs.Account
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly EngineeringThesisDbContext _db;

        public RegisterUserDtoValidator(EngineeringThesisDbContext db)
        {
            _db = db;

            RuleFor(x => x.Email)
                .Must(value => !EmailAlreadyExists(value))
                .WithMessage("This email is already taken")
                .WithErrorCode("EmailAlreadyTaken");

            RuleFor(x => x.NickName)
                .Must(value => !NickAlreadyExists(value))
                .WithMessage("This nick is already taken")
                .WithErrorCode("NickAlreadyTaken");
        }

        private bool EmailAlreadyExists(string email)
        {
            return _db.Users.Any(r => r.Email == email);
        }

        private bool NickAlreadyExists(string nick)
        {
            return _db.Users.Any(r => r.NickName == nick);
        }

    }
}
