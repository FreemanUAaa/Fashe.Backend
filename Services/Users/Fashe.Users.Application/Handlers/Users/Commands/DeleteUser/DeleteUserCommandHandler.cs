using Fashe.Users.Core.Cache;
using Fashe.Users.Core.Exceptions;
using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Models;
using Fashe.Users.Producers.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Handlers.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly ILogger<DeleteUserCommandHandler> logger;

        private readonly IUserProducer userProducer;

        private readonly IDatabaseContext database;

        private readonly IDistributedCache cache;

        public DeleteUserCommandHandler(IDatabaseContext database, IUserProducer userProducer, IDistributedCache cache, ILogger<DeleteUserCommandHandler> logger) =>
            (this.database, this.userProducer, this.cache, this.logger) = (database, userProducer, cache, logger);

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User user = await database.Users.FindAsync(request.UserId);

            if (user == null)
            {
                throw new Exception(ExceptionStrings.NotFound);
            }

            database.Users.Remove(user);
            await database.SaveChangesAsync(cancellationToken);

            await userProducer.UserDeleted(user.Id);

            await cache.RemoveAsync(CacheKeys.UserKey(user.Id));

            logger.LogInformation("User removed successfully");

            return Unit.Value;
        }
    }
}
