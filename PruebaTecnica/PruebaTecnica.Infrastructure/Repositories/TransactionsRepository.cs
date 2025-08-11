using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Contracts.Persistence;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Persistence;
using System.Reflection.Metadata;


namespace PruebaTecnica.Infrastructure.Repositories
{
    public class TransactionsRepository : BaseRepository<Transactions>, ITransactionsRepository
    {
        public TransactionsRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> CantidadReintentos(string pan, string cvv, string expiry, string isoCode)
        {
            var query = _dbContext.Set<Transactions>()
                .Where(x => x.Pan == pan &&
                            x.Cvv == cvv &&
                            x.Expiry == expiry &&
                            x.AuthorizationCode == isoCode);

            var ultimoIntento = await query
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (ultimoIntento == null)
                return 0;

            // Si el último intento fue hace más de 5 minutos, reiniciar el contador como prueba
            if (ultimoIntento.CreatedAt < DateTime.UtcNow.AddMinutes(-5))
                return 0;

            // Contar los intentos en los últimos 5 minutos
            var desde = DateTime.UtcNow.AddMinutes(-5);
            var cantidad = await query
                .Where(x => x.CreatedAt >= desde)
                .CountAsync();

            return cantidad;
        }
    }
}
