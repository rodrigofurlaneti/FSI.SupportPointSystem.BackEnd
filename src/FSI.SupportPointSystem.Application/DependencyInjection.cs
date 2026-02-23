using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace FSI.SupportPointSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<ISellerAppService, SellerAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IVisitAppService, VisitAppService>();
            return services;
        }
    }
}