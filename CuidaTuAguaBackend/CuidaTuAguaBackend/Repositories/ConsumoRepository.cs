using CuidaTuAguaBackend.Data;
using CuidaTuAguaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CuidaTuAguaBackend.Repositories;

public class ConsumoRepository : IConsumoRepository
{
    private readonly AppDbContext _context;

    public ConsumoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HogarExistsAsync(int hogarId)
    {
        return await _context.Hogares.AnyAsync(h => h.Id == hogarId);
    }

    public async Task AddAsync(ConsumoAgua consumo)
    {
        await _context.Consumos.AddAsync(consumo);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<ConsumoAgua>> GetByHogarAndRangeAsync(int hogarId, DateTime desdeUtc, DateTime hastaUtc)
    {
        return await _context.Consumos
            .Where(c => c.HogarId == hogarId)
            .Where(c => c.FechaHora >= desdeUtc && c.FechaHora < hastaUtc)
            .OrderBy(c => c.FechaHora)
            .ToListAsync();
    }
}
