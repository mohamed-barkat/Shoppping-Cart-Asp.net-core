using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Contracts;
using Shop.Application.Contracts.Notifications;
using Shop.Data.DataContext;
using Shop.DataAccess.DataContext;
using Shop.DataAccess.Repositories;
using Shop.DataAccess.Repositories.Notifications;
using Shop.Domain.Models;
using Shop.UI.CartDataStore;
using Shop.UI.EmailServices;
using System.Text.Json.Serialization;

namespace Shop.UI.RegisterServices
{
    public static class RegisterServices
    {



        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultCon"), b => b.MigrationsAssembly("Shop.DataAccess")));
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                //options.Lockout.MaxFailedAccessAttempts = 3;
                //options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(1);
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();


            services.AddTransient<ICartDataBase, CartDatabase>();

            services.AddScoped<ICartSessionData, CartSessionData>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddSingleton<IEmailConfiguration>(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
       
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<INotification, NotificationRepository>();

            

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdmin", policy =>
                {

                    policy.RequireRole("Super-Admin");
                });
                options.AddPolicy("Admin-Panel", policy =>
                {
                    policy.RequireAssertion(context => AuthorizeAccess(context)

                    );
                });
            });
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });
            bool AuthorizeAccess(AuthorizationHandlerContext context)
            {
                return context.User.IsInRole("Admin") || context.User.IsInRole("Super-Admin");

            }
            return services;
        }
    }
}
