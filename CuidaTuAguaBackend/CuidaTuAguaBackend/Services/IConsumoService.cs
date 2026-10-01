using CuidaTuAguaBackend.DTOs;

namespace CuidaTuAguaBackend.Services;

public interface IConsumoService
{
    /// <summary>Registra una lectura de consumo (endpoint de apoyo para cargar datos de prueba).</summary>
    Task<ConsumoResponseDto> RegistrarAsync(ConsumoCreateDto dto);

    /// <summary>HU-016: consulta el consumo de un hogar agregado por período ("day" | "week" | "month").</summary>
    Task<ConsumoConsultaResponseDto> ConsultarAsync(int hogarId, string periodoTexto, DateTime? fechaReferencia);
}
