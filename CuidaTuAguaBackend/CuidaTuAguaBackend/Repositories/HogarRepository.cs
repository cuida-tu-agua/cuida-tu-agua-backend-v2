using CuidaTuAguaBackend.Data;
using CuidaTuAguaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CuidaTuAguaBackend.Repositories;

public class HogarRepository : IHogarRepository
{
    private readonly AppDbContext _context;

    public HogarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Hogar?> GetByIdAsync(int id)
    {
        return await _context.Hogares.FindAsync(id);
    }

    public async Task<IEnumerable<Hogar>> GetAllAsync()
    {
        return await _context.Hogares
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByDireccionAsync(string direccion, int? excludeId = null)
    {
        return await _context.Hogares
            .Where(h => h.Direccion.ToLower() == direccion.ToLower())
            .Where(h => excludeId == null || h.Id != excludeId)
            .AnyAsync();
    }

    public async Task<bool> ExistsByTelefonoAsync(string telefono, int? excludeId = null)
    {
        return await _context.Hogares
            .Where(h => h.Telefono == telefono)
            .Where(h => excludeId == null || h.Id != excludeId)
            .AnyAsync();
    }

    public async Task AddAsync(Hogar hogar)
    {
        await _context.Hogares.AddAsync(hogar);
    }

    public void Update(Hogar hogar)
    {
        _context.Hogares.Update(hogar);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
