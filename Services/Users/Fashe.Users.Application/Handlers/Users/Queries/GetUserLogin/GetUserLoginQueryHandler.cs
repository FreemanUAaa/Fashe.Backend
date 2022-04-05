using Fashe.Users.Application.Helpers;
using Fashe.Users.Core.Exceptions;
using Fashe.Users.Core.Helpers.Passwords;
using Fashe.Users.Core.Interfaces.Database;
using Fashe.Users.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Fashe.Users.Application.Handlers.Users.Queries.GetUserLogin
{
    public class GetUserLoginQueryHandler : IRequestHandler<GetUserLoginQuery, GetUserLoginVm>
    {
        private readonly ILogger<GetUserLoginQueryHandler> logger;

        private readonly IDatabaseContext database;

        private readonly AuthOptions authOptions;

        public GetUserLoginQueryHandler(IDatabaseContext database, IOptions<AuthOptions> options, ILogger<GetUserLoginQueryHandler> logger) =>
            (this.database, this.authOptions, this.logger) = (database, options.Value, logger);

        public async Task<GetUserLoginVm> Handle(GetUserLoginQuery request, CancellationToken cancellationToken)
        {
            User user = await database.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            ClaimsIdentity identity = GetIdentity(user);

            if (identity == null)
            {
                throw new Exception(ExceptionStrings.NotFound);
            }

            string hash = PasswordHasher.Hash(request.Password, user.PasswordSalt);

            if (user.PasswordHash != hash)
            {
                throw new Exception(ExceptionStrings.NotFound);
            }

            DateTime now = DateTime.UtcNow;
            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(authOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            logger.LogInformation("The user has successfully entered");

            return new GetUserLoginVm() { AccessToken = token, UserId = user.Id };
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            if (user == null)
            {
                return null;
            }

            List<Claim> claims = new List<Claim> 
            { 
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", 
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
