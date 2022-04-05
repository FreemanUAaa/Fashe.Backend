using MediatR;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUserLogin
{
    public class GetUserLoginQuery : IRequest<GetUserLoginVm>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
