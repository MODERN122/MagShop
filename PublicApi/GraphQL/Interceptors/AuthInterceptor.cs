using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Infrastructure.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Interceptors
{
    public class AuthInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthInterceptor(
            IServiceProvider serviceProvider)
        { _serviceProvider = serviceProvider; }
        public override ValueTask OnCreateAsync(
            HttpContext context,
            IRequestExecutor requestExecutor, 
            IQueryRequestBuilder builder,
            CancellationToken cancellationToken)
        {
            base.OnCreateAsync(context, requestExecutor, builder,
                   cancellationToken);
            try
            {
                // Decode token
                var authHeader = context.Request.Headers.Single(p => p.Key == "Authorization");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenStr = string.Join("", authHeader.Value.First().Skip(7));
                var key = Encoding.ASCII.GetBytes(ConstantsAPI.JWT_SECRET_KEY);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var token = tokenHandler.ValidateToken(tokenStr, tokenValidationParameters, out var validToken);
                JwtSecurityToken validJwt = validToken as JwtSecurityToken;
                context.User = token;
                //TODO Add finding user with userManager
                builder.TryAddProperty(nameof(ClaimsPrincipal), context.User);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ValueTask.CompletedTask;
        }
    }
}
