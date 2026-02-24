using FluentValidation;
using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FSI.SupportPointSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // App Services Existentes
            services.AddScoped<ISellerAppService, SellerAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IVisitAppService, VisitAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ISalesTeamAppService, SalesTeamAppService>();

            return services;
        }
    }
}