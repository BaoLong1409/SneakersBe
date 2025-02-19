using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Interfaces;
using DataAccess.Repositories;
using DataAccess.UnitOfWork;
using DataAccess.AutoMapper;
using Domain.Entities;
using DataAccess.DbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sneakers.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Sneakers.Features.Queries.FeatureProducts;
using MediatR;
using Sneakers.Handler.QueriesHandler.FeatureProductsHandler;
using Domain.ViewModel;
using Sneakers.Features.Queries.Products;
using Sneakers.Handler.QueriesHandler.ProductsHandler;


namespace Sneakers.AddServicesCollection
{
    public static class AddTransientNService
    {
        public static void ConfigureTransient(this IServiceCollection services)
        {
            services.AddTransient(typeof (IGenericRepository<>), typeof (GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IRequestHandler<GetAllFeatureProducts, List<FeatureProductModel>>, GetAllFeatureProductsHandler>();
            services.AddTransient<IRequestHandler<GetAllProducts, IEnumerable<ShowProductsDto>>, GetAllProductsHandler>();
            services.AddTransient<IRequestHandler<GetRecommendProducts, IEnumerable<ShowProductsDto>>, GetRecommendProductsHandler>();
        }

        public static void ConfigureServices(this IServiceCollection service, IConfiguration config)
        {
            service.AddHttpClient();
            service.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            service.AddSignalR();
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            service.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<Role>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SneakersDbContext>();

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JWT:Issuer"],
                        ValidAudience = config["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/chathub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
