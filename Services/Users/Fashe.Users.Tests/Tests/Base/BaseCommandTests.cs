using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Producers.Interfaces;
using Fashe.Users.Tests.Helpers.Database;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fashe.Users.Tests.Tests.Base
{
    public abstract class BaseCommandTests<TLogger>
    {
        public readonly IUserProducer UserProducer;

        public readonly IDatabaseContext Database;

        public readonly ILogger<TLogger> Logger;

        public readonly IDistributedCache Cache;

        public BaseCommandTests()
        {
            Database = DatabaseContextFactory.Create();

            UserProducer = new Mock<IUserProducer>().Object;

            Logger = new Mock<ILogger<TLogger>>().Object;

            Cache = new Mock<IDistributedCache>().Object;
        }
    }
}
