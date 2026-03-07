using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Domain.Services;
using FSI.SupportPointSystem.Infrastructure.Context;
using FSI.SupportPointSystem.Infrastructure.ExternalServices; // Adicione o namespace onde está o OpenCnpjService
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
            services.AddSingleton<DbConnectionFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILocationService, LocationService>();

            // --- Novo Registro para consulta de CNPJ ---
            services.AddHttpClient<IEnterpriseExternalService, OpenCnpjService>(client =>
            {
                client.BaseAddress = new Uri("https://api.opencnpj.org/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

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