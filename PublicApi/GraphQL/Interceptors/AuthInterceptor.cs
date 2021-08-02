using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using PublicApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.GraphQL.Interceptors
{
    public class AuthInterceptor : DefaultHttpRequestInterceptor
    {
        public override ValueTask OnCreateAsync(
            HttpContext context,
            IRequestExecutor requestExecutor, 
            IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            requestBuilder.TrySetServices(context.RequestServices);
            requestBuilder.TryAddProperty(nameof(HttpContext), context);
            requestBuilder.TryAddProperty(nameof(ClaimsPrincipal), context.User);
            requestBuilder.TryAddProperty(nameof(CancellationToken), context.RequestAborted);

            if (context.IsTracingEnabled())
            {
                requestBuilder.TryAddProperty(WellKnownContextData.EnableTracing, true);
            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }
    }
}
