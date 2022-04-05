using Fashe.Users.Core.Cache;
using Fashe.Users.Core.Interfaces.Caching;
using MediatR;
using System;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserVm>, ICacheableMediatorQuery
    {
        public Guid UserId { get; set; }


        public string CacheKey => CacheKeys.UserKey(UserId);
    }
}
