using Fashe.Users.Application.Handlers.Users.Queries.GetUserLogin;
using Fashe.Users.Application.Helpers;
using Fashe.Users.Core.Helpers.Passwords;
using Fashe.Users.Core.Models;
using Fashe.Users.Tests.Tests.Base;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fashe.Users.Tests.Tests.Users.Queries
{
    public class GetUserLoginQueryHandlerTests : BaseQueryTests<GetUserLoginQueryHandler>
    {
        private readonly IOptions<AuthOptions> options;

        private readonly byte[] salt;

        public GetUserLoginQueryHandlerTests()
        {
            AuthOptions auth = new AuthOptions()
            {
                Issuer = "test-issuer",
                Audience = "test-audience",
                Key = "mysupersecret_secretkey!123",
                Lifetime = 120,
            };

            options = Options.Create(auth);

            salt = PasswordHasher.GetNewSalt();
        }

        [Fact]
        public async void GetUserLoginQueryHandlerSuccess()
        {
            User user = await CreateAndSaveTestUser("test-password", salt);
            GetUserLoginQueryHandler handler = new GetUserLoginQueryHandler(Database, options, Logger);
            GetUserLoginQuery query = new GetUserLoginQuery()
            { 
                Email = user.Email,
                Password = "test-password",
            };


            GetUserLoginVm vm = await handler.Handle(query, CancellationToken.None);


            Assert.NotNull(vm);
            Assert.NotNull(vm.AccessToken);
            vm.UserId.ShouldBe(user.Id);
        }

        [Fact]
        public async void GetUserLoginQueryHandlerFailOnWrongPassword()
        {
            User user = await CreateAndSaveTestUser("test-password", salt);
            GetUserLoginQueryHandler handler = new GetUserLoginQueryHandler(Database, options, Logger);
            GetUserLoginQuery query = new GetUserLoginQuery()
            {
                Email = user.Email,
                Password = "wrong-password",
            };


            await Assert.ThrowsAsync<Exception>(async () => 
                await handler.Handle(query, CancellationToken.None));
        }

        private async Task<User> CreateAndSaveTestUser(string password, byte[] salt)
        {
            string hash = PasswordHasher.Hash(password, salt);

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "test-email",
                PasswordSalt = salt,
                PasswordHash = hash,
            };

            Database.Users.Add(user);
            await Database.SaveChangesAsync();

            return user;
        }
    }
}
