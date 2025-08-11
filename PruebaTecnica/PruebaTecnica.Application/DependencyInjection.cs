using Microsoft.Extensions.DependencyInjection;
using PruebaTecnica.Application.Features.Commands.CreateTransaction;

namespace PruebaTecnica.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            /* Transactions Commands */
            services.AddScoped<CreateTransactionsHandler>();

            return services;

        }
    }
}
