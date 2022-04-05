using System;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUserLogin
{
    public class GetUserLoginVm
    {
        public string AccessToken { get; set; }

        public Guid UserId { get; set; }
    }
}
