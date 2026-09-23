using CuidaTuAguaBackend.Common.Exceptions;
using CuidaTuAguaBackend.DTOs;
using CuidaTuAguaBackend.Models;
using CuidaTuAguaBackend.Repositories;

namespace CuidaTuAguaBackend.Services;

public class HogarService : IHogarService
{
    private readonly IHogarRepository _repository;

    public HogarService(IHogarRepository repository)
    {
        _repository = repository;
    }

    public async Task<HogarResponseDto> RegistrarAsync(HogarCreateDto dto)
    {
        if (await _repository.ExistsByDireccionAsync(dto.Direccion))
        {
            throw new ConflictException("Ya existe un hogar registrado con esa dirección.");
        }

        if (await _repository.ExistsByTelefonoAsync(dto.Telefono))
        {
            throw new ConflictException("Ya existe un hogar registrado con ese número de teléfono.");
        }

        var hogar = new Hogar
        {
            UsuarioId = dto.UsuarioId,
            Nombre = dto.Nombre.Trim(),
            Direccion = dto.Direccion.Trim(),
            Telefono = dto.Telefono.Trim(),
            Pais = dto.Pais.Trim(),
            Estrato = dto.Estrato.Trim(),
            Habitantes = dto.Habitantes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await _repository.AddAsync(hogar);
        await _repository.SaveChangesAsync();

        return MapToResponse(hogar);
    }

    public async Task<HogarResponseDto> EditarAsync(int id, HogarUpdateDto dto)
    {
        var hogar = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"No se encontró un hogar con id {id}.");

        if (await _repository.ExistsByDireccionAsync(dto.Direccion, excludeId: id))
        {
            throw new ConflictException("Ya existe otro hogar registrado con esa dirección.");
        }

        if (await _repository.ExistsByTelefonoAsync(dto.Telefono, excludeId: id))
        {
            throw new ConflictException("Ya existe otro hogar registrado con ese número de teléfono.");
        }

        hogar.Nombre = dto.Nombre.Trim();
        hogar.Direccion = dto.Direccion.Trim();
        hogar.Telefono = dto.Telefono.Trim();
        hogar.Pais = dto.Pais.Trim();
        hogar.Estrato = dto.Estrato.Trim();
        hogar.Habitantes = dto.Habitantes;
        hogar.UsuarioId = dto.UsuarioId;
        hogar.UpdatedAt = DateTime.UtcNow;

        _repository.Update(hogar);
        await _repository.SaveChangesAsync();

        return MapToResponse(hogar);
    }

    public async Task<HogarResponseDto> ObtenerPorIdAsync(int id)
    {
        var hogar = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"No se encontró un hogar con id {id}.");

        return MapToResponse(hogar);
    }

    public async Task<IEnumerable<HogarResponseDto>> ObtenerTodosAsync()
    {
        var hogares = await _repository.GetAllAsync();
        return hogares.Select(MapToResponse);
    }

    private static HogarResponseDto MapToResponse(Hogar hogar) => new()
    {
        Id = hogar.Id,
        UsuarioId = hogar.UsuarioId,
        Nombre = hogar.Nombre,
        Direccion = hogar.Direccion,
        Telefono = hogar.Telefono,
        Pais = hogar.Pais,
        Estrato = hogar.Estrato,
        Habitantes = hogar.Habitantes,
        CreatedAt = hogar.CreatedAt,
        UpdatedAt = hogar.UpdatedAt,
    };
}
