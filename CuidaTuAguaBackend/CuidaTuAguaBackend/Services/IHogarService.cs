using CuidaTuAguaBackend.DTOs;

namespace CuidaTuAguaBackend.Services;

public interface IHogarService
{
    /// <summary>HU-008: Registrar un lugar (hogar).</summary>
    Task<HogarResponseDto> RegistrarAsync(HogarCreateDto dto);

    /// <summary>HU-009: Editar lugar (hogar).</summary>
    Task<HogarResponseDto> EditarAsync(int id, HogarUpdateDto dto);

    Task<HogarResponseDto> ObtenerPorIdAsync(int id);

    Task<IEnumerable<HogarResponseDto>> ObtenerTodosAsync();
}
