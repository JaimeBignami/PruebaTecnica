using PruebaTecnica.Domain.Entities;


namespace PruebaTecnica.Application.Contracts.Persistence
{
    public interface ITransactionsRepository : IBaseRepository<Transactions>
    {
        // Métodos adicionales si lo necesitas
        Task<int> CantidadReintentos(string pan, string cvv, string expiry, string isoCode);
    }
}
