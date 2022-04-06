using Fashe.Users.Application.Handlers.Users.Queries.GetUser;
using Fashe.Users.Core.Models;
using Fashe.Users.Tests.Tests.Base;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fashe.Users.Tests.Tests.Users.Queries
{
    public class GetUserQueryHandlerTests : BaseQueryTests<GetUserQueryHandler>
    {
        [Fact]
        public async void GetUserQueryHandlerSuccess()
        {
            User user = await CreateAndSaveTestUser();
            GetUserQueryHandler handler = new GetUserQueryHandler(Database, Mapper, Logger);
            GetUserQuery query = new GetUserQuery() { UserId = user.Id };


            GetUserVm vm = await handler.Handle(query, CancellationToken.None);


            Assert.NotNull(vm);
        }

        [Fact]
        public async void GetUserQueryHandlerFailOnWrongId()
        {
            GetUserQueryHandler handler = new GetUserQueryHandler(Database, Mapper, Logger);
            GetUserQuery query = new GetUserQuery();


            await Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(query, CancellationToken.None));
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
