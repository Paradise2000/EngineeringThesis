using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EngineeringThesisAPI.DTOs.Account
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        private readonly EngineeringThesisDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserIdProvider _userIdProvider;

        public ChangePasswordDtoValidator(EngineeringThesisDbContext db, IPasswordHasher<User> passwordHasher, IUserIdProvider userIdProvider)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _userIdProvider = userIdProvider;

            RuleFor(x => x.CurrentPassword)
                .Must(value => IsPasswordCorrect(value) == true)
                .WithMessage("Wrong current password")
                .WithErrorCode("WrongPassword");

            RuleFor(x => x.NewPassword)
                .Equal(y => y.RetypedNewPassword)
                .WithMessage("Passwords are not the same")
                .WithErrorCode("PasswordNotSame");
        }

        private bool IsPasswordCorrect(string password)
        {
            var user = _db.Users.FirstOrDefault(r => r.Id == _userIdProvider.GetUserId());

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }
    }
}
