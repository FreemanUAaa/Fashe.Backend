using Fashe.Users.Application.Handlers.Users.Commands.DeleteUser;
using Fashe.Users.Core.Models;
using Fashe.Users.Tests.Tests.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fashe.Users.Tests.Tests.Users.Commands
{
    public class DeleteUserCommandHandlerTests : BaseCommandTests<DeleteUserCommandHandler>
    {
        [Fact]
        public async void DeleteUserCommandHandlerSuccess()
        {
            User user = await CreateAndSaveTestUser();
            DeleteUserCommandHandler handler = new DeleteUserCommandHandler(Database, UserProducer, Cache, Logger);
            DeleteUserCommand command = new DeleteUserCommand() { UserId = user.Id };


            await handler.Handle(command, CancellationToken.None);


            Assert.Null(await Database.Users.FindAsync(user.Id));
        }

        [Fact]
        public async void DeleteUserCommandHandlerFailOnWrongId()
        {
            DeleteUserCommandHandler handler = new DeleteUserCommandHandler(Database, UserProducer, Cache, Logger);
            DeleteUserCommand command = new DeleteUserCommand();


            await Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        private async Task<User> CreateAndSaveTestUser()
        {
            User user = new User() { Id = Guid.NewGuid(), };

            Database.Users.Add(user);
            await Database.SaveChangesAsync();

            return user;
        }
    }
}
