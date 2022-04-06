using Fashe.Users.Application.Handlers.Users.Commands.CreateUser;
using Fashe.Users.Core.Models;
using Fashe.Users.Tests.Tests.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fashe.Users.Tests.Tests.Users.Commands
{
    public class CreateUserCommandHandlerTests : BaseCommandTests<CreateUserCommandHandler>
    {
        [Fact]
        public async void CreateUserCommandHandlerSuccess()
        {
            CreateUserCommandHandler handler = new CreateUserCommandHandler(Database, UserProducer, Logger);
            CreateUserCommand command = new CreateUserCommand()
            {
                Name = "test-name",
                Role = "test-role",
                Email = "test-email",
                Password = "test-password",
            };


            Guid userId = await handler.Handle(command, CancellationToken.None);


            Assert.NotNull(await Database.Users.FindAsync(userId));
        }

        [Fact]
        public async void CreateUserCommandHandlerFailOnWrongEmail()
        {
            User user = await CreateAndSaveTestUser();
            CreateUserCommandHandler handler = new CreateUserCommandHandler(Database, UserProducer, Logger);
            CreateUserCommand command = new CreateUserCommand() { Email = user.Email, };


            await Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        private async Task<User> CreateAndSaveTestUser()
        {
            User user = new User() { Email = "test-email", };

            Database.Users.Add(user);
            await Database.SaveChangesAsync();

            return user;
        }
    }
}
