using CuidaTuAguaBackend.Models;

namespace CuidaTuAguaBackend.Repositories;

public interface IConsumoRepository
{
    Task<bool> HogarExistsAsync(int hogarId);

    Task AddAsync(ConsumoAgua consumo);

    Task<bool> SaveChangesAsync();

    /// <summary>Lecturas de un hogar entre [desdeUtc, hastaUtc).</summary>
    Task<List<ConsumoAgua>> GetByHogarAndRangeAsync(int hogarId, DateTime desdeUtc, DateTime hastaUtc);
}
