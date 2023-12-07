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
                    if(db.Comments.FirstOrDefault(r => r.UserId == id.GetUserId()) != null)
                    {
                        context.AddFailure("The specified user has already added a comment");
                    }
                });
        }
    }
}
