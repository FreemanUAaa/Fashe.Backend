using FluentValidation;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUserLogin
{
    public class GetUserLoginQueryValidator : AbstractValidator<GetUserLoginQuery>
    {
        public GetUserLoginQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
