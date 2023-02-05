using Bll.Services;
using Dal.Repository;
using Dal.Repository.Interfaces;
using Dal.UnitOfWork;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bll.Infrastructure
{
    public static class BllConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ILotImageRepository, LotImageRepository>();
            services.AddScoped<ILotRepository, LotRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSenderService>();

            // Services
            services.AddScoped<LotService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<LotImageService>();
            services.AddScoped<OrderService>();
        }
    }
}