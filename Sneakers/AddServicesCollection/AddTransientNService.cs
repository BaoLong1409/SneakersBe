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
using Sneakers.Features.Queries.Products;
using Sneakers.Handler.QueriesHandler.ProductsHandler;
using Sneakers.Services.CartService;
using Sneakers.Services.SizeService;
using Sneakers.Services.ColorService;
using Sneakers.Services.ProductService;
using Domain.ViewModel.Product;
using Sneakers.Services.OrderService;
using Sneakers.Services.VnpayService;
using Sneakers.Services.ShippingService;
using Sneakers.Services.PaymentService;
using Sneakers.Features.Queries.Order;
using Domain.ViewModel.Order;
using Sneakers.Handler.QueriesHandler.OrderHandler;
using Sneakers.Services.UserService;
using Sneakers.Services.CategoryService;
using Sneakers.Services.OTPService;
using Sneakers.Features.Queries.ProductReview;
using Domain.ViewModel.ProductReview;
using Sneakers.Handler.QueriesHandler.ProductReviewHandler;


namespace Sneakers.AddServicesCollection
{
    public static class AddTransientNService
    {
        public static void ConfigureTransient(this IServiceCollection services)
        {
            services.AddTransient(typeof (IGenericRepository<>), typeof (GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductCartRepository, ProductCartRepository>();
            services.AddScoped<IProductQuantityRepository, ProductQuantityRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<IProductReviewImageRepository, ProductReviewImageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<CartService>();
            services.AddScoped<UserService>();
            services.AddScoped<SizeService>();
            services.AddScoped<ColorService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductReviewService>();
            services.AddScoped<OrderService>();
            services.AddScoped<OrderDetailService>();
            services.AddScoped<ShippingService>();
            services.AddScoped<VnpayService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<OTPService>();

            services.AddScoped<IRequestHandler<GetAllFeatureProducts, List<FeatureProductModel>>, GetAllFeatureProductsHandler>();
            services.AddScoped<IRequestHandler<GetAllProducts, IEnumerable<AllProductsDto>>, GetAllProductsHandler>();
            services.AddScoped<IRequestHandler<GetAllProductsWithCondition, IEnumerable<AllProductsDto>>, GetAllProductsWithConditionHandler>();
            services.AddScoped<IRequestHandler<GetAllProductsByCategory, IEnumerable<AllProductsDto>>, GetAllProductsByCategoryHandler>();
            services.AddScoped<IRequestHandler<SearchAllProducts, IEnumerable<AllProductsDto>>, SearchAllProductsHandler>();
            services.AddScoped<IRequestHandler<GetRecommendProducts, IEnumerable<ShowProductsDto>>, GetRecommendProductsHandler>();
            services.AddScoped<IRequestHandler<GetProductById, DetailProductDto>, GetProductByIdHandler>();
            services.AddScoped<IRequestHandler<GetOrderInfo, OrderInfoDto>, GetOrderInfoHandler>();
            services.AddScoped<IRequestHandler<GetAllOrders, IEnumerable <AllOrdersDto>>, GetAllOrdersHandler>();
            services.AddScoped<IRequestHandler<GetProductsAreWaittingReview, IEnumerable<ProductsAreWaitingReviewDto>>, GetProductsAreWaitingReviewHandler>();
            services.AddScoped<IRequestHandler<GetCommentOfProduct, IEnumerable<ProductReviewDto>>, GetCommentOfProductHandler>();

            services.AddHttpContextAccessor();
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
