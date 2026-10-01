using CuidaTuAguaBackend.Common.Enums;
using CuidaTuAguaBackend.Common.Exceptions;
using CuidaTuAguaBackend.DTOs;
using CuidaTuAguaBackend.Models;
using CuidaTuAguaBackend.Repositories;

namespace CuidaTuAguaBackend.Services;

public class ConsumoService : IConsumoService
{
    private readonly IConsumoRepository _repository;

    public ConsumoService(IConsumoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsumoResponseDto> RegistrarAsync(ConsumoCreateDto dto)
    {
        if (!await _repository.HogarExistsAsync(dto.HogarId))
        {
            throw new NotFoundException($"No se encontró un hogar con id {dto.HogarId}.");
        }

        var fechaHoraUtc = dto.FechaHora.HasValue
            ? DateTime.SpecifyKind(dto.FechaHora.Value, DateTimeKind.Utc)
            : DateTime.UtcNow;

        var consumo = new ConsumoAgua
        {
            HogarId = dto.HogarId,
            FechaHora = fechaHoraUtc,
            ValorM3 = dto.ValorM3,
            CreatedAt = DateTime.UtcNow,
        };

        await _repository.AddAsync(consumo);
        await _repository.SaveChangesAsync();

        return new ConsumoResponseDto
        {
            Id = consumo.Id,
            HogarId = consumo.HogarId,
            FechaHora = consumo.FechaHora,
            ValorM3 = consumo.ValorM3,
        };
    }

    public async Task<ConsumoConsultaResponseDto> ConsultarAsync(int hogarId, string periodoTexto, DateTime? fechaReferencia)
    {
        if (!await _repository.HogarExistsAsync(hogarId))
        {
            throw new NotFoundException($"No se encontró un hogar con id {hogarId}.");
        }

        if (!TryParsePeriodo(periodoTexto, out var periodo))
        {
            throw new BadRequestException("El parámetro 'periodo' debe ser 'day', 'week' o 'month'.");
        }

        // Fecha de referencia: el día que el usuario quiere mirar. Por defecto, hoy (UTC).
        var referencia = (fechaReferencia ?? DateTime.UtcNow).Date;

        var (inicio, fin) = CalcularRango(periodo, referencia);

        var lecturas = await _repository.GetByHogarAndRangeAsync(hogarId, inicio, fin);

        // Agregación (HU-016): por hora si es "day", por día si es "week" o "month".
        var puntos = periodo == PeriodoConsulta.Day
            ? AgregarPorHora(lecturas, inicio)
            : AgregarPorDia(lecturas, inicio, fin);

        var total = puntos.Sum(p => p.TotalM3);

        return new ConsumoConsultaResponseDto
        {
            HogarId = hogarId,
            Periodo = periodoTexto.Trim().ToLowerInvariant(),
            FechaInicio = inicio,
            FechaFin = fin,
            TotalM3 = total,
            PromedioM3 = puntos.Count > 0 ? Math.Round(total / puntos.Count, 4) : 0,
            Datos = puntos,
        };
    }

    /// <summary>
    /// Calcula el rango [inicio, fin) según el período pedido, tomando "referencia"
    /// como el día que se quiere consultar.
    /// - day: ese mismo día completo.
    /// - week: la semana (lunes a domingo) que contiene a "referencia".
    /// - month: el mes calendario que contiene a "referencia".
    /// </summary>
    private static (DateTime inicio, DateTime fin) CalcularRango(PeriodoConsulta periodo, DateTime referencia)
    {
        switch (periodo)
        {
            case PeriodoConsulta.Day:
                return (referencia, referencia.AddDays(1));

            case PeriodoConsulta.Week:
                var diasDesdeLunes = ((int)referencia.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                var inicioSemana = referencia.AddDays(-diasDesdeLunes);
                return (inicioSemana, inicioSemana.AddDays(7));

            case PeriodoConsulta.Month:
                var inicioMes = new DateTime(referencia.Year, referencia.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                return (inicioMes, inicioMes.AddMonths(1));

            default:
                throw new ArgumentOutOfRangeException(nameof(periodo), periodo, "Período no soportado.");
        }
    }

    /// <summary>24 buckets, uno por hora del día.</summary>
    private static List<ConsumoPuntoDto> AgregarPorHora(List<ConsumoAgua> lecturas, DateTime inicioDia)
    {
        var buckets = new List<ConsumoPuntoDto>(24);

        for (var hora = 0; hora < 24; hora++)
        {
            var inicioBucket = inicioDia.AddHours(hora);
            var finBucket = inicioBucket.AddHours(1);

            var total = lecturas
                .Where(l => l.FechaHora >= inicioBucket && l.FechaHora < finBucket)
                .Sum(l => l.ValorM3);

            buckets.Add(new ConsumoPuntoDto
            {
                Etiqueta = inicioBucket.ToString("HH:00"),
                Inicio = inicioBucket,
                TotalM3 = total,
            });
        }

        return buckets;
    }

    /// <summary>Un bucket por cada día dentro de [inicio, fin).</summary>
    private static List<ConsumoPuntoDto> AgregarPorDia(List<ConsumoAgua> lecturas, DateTime inicio, DateTime fin)
    {
        var buckets = new List<ConsumoPuntoDto>();

        for (var dia = inicio; dia < fin; dia = dia.AddDays(1))
        {
            var finDia = dia.AddDays(1);

            var total = lecturas
                .Where(l => l.FechaHora >= dia && l.FechaHora < finDia)
                .Sum(l => l.ValorM3);

            buckets.Add(new ConsumoPuntoDto
            {
                Etiqueta = dia.ToString("yyyy-MM-dd"),
                Inicio = dia,
                TotalM3 = total,
            });
        }

        return buckets;
    }

    private static bool TryParsePeriodo(string? value, out PeriodoConsulta periodo)
    {
        switch (value?.Trim().ToLowerInvariant())
        {
            case "day":
                periodo = PeriodoConsulta.Day;
                return true;
            case "week":
                periodo = PeriodoConsulta.Week;
                return true;
            case "month":
                periodo = PeriodoConsulta.Month;
                return true;
            default:
                periodo = default;
                return false;
        }
    }
}
