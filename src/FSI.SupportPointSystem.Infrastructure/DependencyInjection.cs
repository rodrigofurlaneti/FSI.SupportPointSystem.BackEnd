using Microsoft.Extensions.DependencyInjection;
using FSI.SupportPoint.Domain.Interfaces.Repositories;
using FSI.SupportPoint.Infrastructure.Repositories;
using FSI.SupportPoint.Infrastructure.Context;

namespace FSI.SupportPointSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<DbConnectionFactory>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            return services;
        }
    }
}

