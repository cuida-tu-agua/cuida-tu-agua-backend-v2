using CuidaTuAguaBackend.Models;

namespace CuidaTuAguaBackend.Repositories;

public interface IHogarRepository
{
    Task<Hogar?> GetByIdAsync(int id);

    Task<IEnumerable<Hogar>> GetAllAsync();

    Task<bool> ExistsByDireccionAsync(string direccion, int? excludeId = null);

    Task<bool> ExistsByTelefonoAsync(string telefono, int? excludeId = null);

    Task AddAsync(Hogar hogar);

    void Update(Hogar hogar);

    Task<bool> SaveChangesAsync();
}
