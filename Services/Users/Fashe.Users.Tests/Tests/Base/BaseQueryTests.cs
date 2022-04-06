using AutoMapper;
using Fashe.Users.Application;
using Fashe.Users.Application.Helpers.Mapper;
using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Interfaces.Mapper;
using Fashe.Users.Database;
using Fashe.Users.Tests.Helpers.Database;
using Microsoft.Extensions.Logging;
using Moq;

namespace Fashe.Users.Tests.Tests.Base
{
    public abstract class BaseQueryTests<TLogger>
    {
        public readonly IDatabaseContext Database;

        public readonly ILogger<TLogger> Logger;

        public readonly IMapper Mapper;

        public BaseQueryTests()
        {
            Database = DatabaseContextFactory.Create();

            Logger = new Mock<ILogger<TLogger>>().Object;

            MapperConfiguration configurationProvider = new MapperConfiguration(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(typeof(DatabaseContext).Assembly));
                config.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly));
                config.AddProfile(new AssemblyMappingProfile(typeof(Middlewares).Assembly));
            });

            Mapper = configurationProvider.CreateMapper();
        }
    }
}
