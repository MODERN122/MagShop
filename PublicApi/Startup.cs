using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.UrlConfiguration;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ApplicationCore;
using Infrastructure.Services;
using Infrastructure.Logging;
using PublicApi.GraphQL;
using PublicApi.GraphQL.Interceptors;
using PublicApi.GraphQL.Users;
using PublicApi.GraphQL.Authentication;
using HotChocolate.AspNetCore.Interceptors;
using HotChocolate.AspNetCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading;
using PublicApi.GraphQL.Products;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using PublicApi.GraphQL.Orders;

namespace PublicApi
{
    public class Startup
    {
        private string CORS_POLICY = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<UserAuthAccess, IdentityRole>()
                       .AddEntityFrameworkStores<MagShopContext>()
                       .AddDefaultTokenProviders();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            //services.Configure<CatalogSettings>(Configuration);
            //services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<CatalogSettings>()));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            var baseUrlConfig = new BaseUrlConfiguration();
            Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);
            services.AddScoped<IFileSystem, WebFileSystemService>(x => new WebFileSystemService($"{baseUrlConfig.WebBase}File")); 

            services.AddPooledDbContextFactory<MagShopContext>(x =>
            {
#if RELEASE
                x.UseSqlServer(Configuration.GetConnectionString("MagShopDBConnectionDocker")));
#else
                x.UseSqlServer(Configuration.GetConnectionString("MagShopDBConnection"));
#endif
                x.EnableSensitiveDataLogging();
            });

            services.AddScoped(x => x.GetRequiredService<IDbContextFactory<MagShopContext>>().CreateDbContext());

            services.AddMemoryCache();

            var key = Encoding.ASCII.GetBytes(ConstantsAPI.JWT_SECRET_KEY);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(config =>
                        {
                            config.RequireHttpsMetadata = false;
                            config.SaveToken = true;
                            config.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                        });

            services.AddAuthorization();
            //TODO Сделать политику, в которой просматривать личную информацию и прочие действия сможет только тот чья это информация
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("SelfAuthorize", policy => policy.AddRequirements(new SelfAuthorize()));
            //});

            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_POLICY,
                                  builder =>
                                  {
                                      builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'));
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                  });
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddControllers();

            services.AddMediatR(typeof(User).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.EnableAnnotations();
                c.SchemaFilter<CustomSchemaFilters>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();
            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddTypeExtension<UserQueries>()
                .AddTypeExtension<ProductQueries>()
                .AddTypeExtension<OrderQueries>()
                .AddTypeExtension<AuthenticationMutations>()
                .AddHttpRequestInterceptor((context, executor, builder, cancellationToken) => {
                    try
                    {
                        // Decode token
                        var authHeader = context.Request.Headers.Single(p => p.Key == "Authorization");
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenStr = string.Join("", authHeader.Value.First().Skip(7));
                        var token = tokenHandler.ReadJwtToken(tokenStr);
                        // Get username and roles
                        var username = token.Claims.First(x=>x.Type== "unique_name")?.Value;
                        // Set User identity
                        context.User = new GenericPrincipal(new GenericIdentity(username), token.Claims.Select(x=>x.Value).ToArray());
                        builder.TryAddProperty(nameof(ClaimsPrincipal), context.User);
                    }
                    catch (Exception ex) { }
                    return ValueTask.CompletedTask;
                });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CORS_POLICY);

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            }); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
