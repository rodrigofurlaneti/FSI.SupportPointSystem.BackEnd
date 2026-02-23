using Microsoft.Extensions.DependencyInjection;
using FSI.SupportPoint.Application.Interfaces;
using FSI.SupportPoint.Application.Services;
using FSI.SupportPoint.Application.Validations;
using FluentValidation;
using FSI.SupportPointSystem.Application.Dtos;
namespace FSI.SupportPointSystemApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IVisitAppService, VisitAppService>();
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<ISellerAppService, SellerAppService>(); 
            services.AddValidatorsFromAssemblyContaining<CheckinRequestValidator>();
            return services;
        }
    }
}