using Fashe.Users.Core.Exceptions;
using Fashe.Users.Core.Helpers.Passwords;
using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Models;
using Fashe.Users.Producers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Handlers.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly ILogger<CreateUserCommandHandler> logger;

        private readonly IUserProducer userProducer;

        private readonly IDatabaseContext database;

        public CreateUserCommandHandler(IDatabaseContext database, IUserProducer userProducer, ILogger<CreateUserCommandHandler> logger) =>
            (this.database, this.userProducer, this.logger) = (database, userProducer, logger);

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (database.Users.Any(x => x.Email == request.Email))
            {
                throw new Exception(ExceptionStrings.EmailAlreadyUsed);
            }

            byte[] salt = PasswordHasher.GetNewSalt();
            string hash = PasswordHasher.Hash(request.Password, salt);

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Role = request.Role,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
            };

            database.Users.Add(user);
            await database.SaveChangesAsync(cancellationToken);

            await userProducer.UserCreated(user.Id);

            logger.LogInformation("User created successfully");

            return user.Id;
        }
    }
}
