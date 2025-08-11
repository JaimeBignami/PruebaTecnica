using Microsoft.Extensions.DependencyInjection;
using PruebaTecnica.Application.Contracts.Infrastructure;
using PruebaTecnica.Application.Contracts.Persistence;
using PruebaTecnica.Application.Features.Commands.CreateTransaction;
using PruebaTecnica.Infrastructure.Repositories;
using PruebaTecnica.Infrastructure.Services;

namespace PruebaTecnica.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            /* Transactions Commands */
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();

            services.AddScoped<IMockService, MockService>();

            return services;

        }
    }
}
