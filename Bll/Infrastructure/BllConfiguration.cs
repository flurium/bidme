using Bll.Services;
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

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailSender, EmailSenderService>();

            // Services
        }
    }
}