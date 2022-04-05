using AutoMapper;
using Fashe.Users.Core.Exceptions;
using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserVm>
    {
        private readonly ILogger<GetUserQueryHandler> logger;

        private readonly IDatabaseContext database;

        private readonly IMapper mapper;

        public GetUserQueryHandler(IDatabaseContext database, IMapper mapper, ILogger<GetUserQueryHandler> logger) =>
            (this.database, this.mapper, this.logger) = (database, mapper, logger);

        public async Task<GetUserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User user = await database.Users.FindAsync(request.UserId);

            if (user == null)
            {
                throw new Exception(ExceptionStrings.NotFound);
            }

            logger.LogInformation("User successfully received");

            return mapper.Map<GetUserVm>(user);
        }
    }
}
