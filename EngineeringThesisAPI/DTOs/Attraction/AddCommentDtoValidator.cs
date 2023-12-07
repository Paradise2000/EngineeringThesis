using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;
using FluentValidation;

namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
    {
        public AddCommentDtoValidator(EngineeringThesisDbContext db, IUserIdProvider id)
        {
            RuleFor(x => x.AttractionId)
                .Custom((value, context) =>
                {
                    if(db.Comments.FirstOrDefault(r => r.UserId == id.GetUserId() && r.AttractionId == value) != null)
                    {
                        context.AddFailure("The specified user has already added a comment");
                    }
                });
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithErrorCode("INT")
                .WithMessage("The number must be an integer from 1 to 5");
        }
    }
}
