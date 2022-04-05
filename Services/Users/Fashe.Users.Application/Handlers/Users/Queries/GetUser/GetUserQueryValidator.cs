using FluentValidation;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUser
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
