using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Domain.Services;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.Repositories;
using FSI.SupportPointSystem.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FSI.SupportPointSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Infra & Services
            services.AddScoped<DbConnectionFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocationService, LocationService>();

            // Repositories
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISalesTeamRepository, SalesTeamRepository>();
            return services;
        }
    }
}