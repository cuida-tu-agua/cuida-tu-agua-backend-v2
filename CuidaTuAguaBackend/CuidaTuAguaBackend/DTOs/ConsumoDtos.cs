using System.ComponentModel.DataAnnotations;

namespace CuidaTuAguaBackend.DTOs;

/// <summary>Datos para registrar una lectura de consumo (endpoint de apoyo, para poder cargar datos de prueba de HU-016).</summary>
public class ConsumoCreateDto
{
    [Required(ErrorMessage = "El id del hogar es obligatorio")]
    public int HogarId { get; set; }

    /// <summary>Si no se envía, se usa la fecha/hora actual (UTC).</summary>
    public DateTime? FechaHora { get; set; }

    [Required(ErrorMessage = "El valor consumido es obligatorio")]
    [Range(0.0001, 100000, ErrorMessage = "El valor debe ser mayor a 0")]
    public decimal ValorM3 { get; set; }
}

/// <summary>Forma en la que se devuelve una lectura individual.</summary>
public class ConsumoResponseDto
{
    public int Id { get; set; }
    public int HogarId { get; set; }
    public DateTime FechaHora { get; set; }
    public decimal ValorM3 { get; set; }
}

/// <summary>Un punto agregado del gráfico: una hora (period=day) o un día (period=week/month).</summary>
public class ConsumoPuntoDto
{
    /// <summary>Etiqueta lista para mostrar: "00:00".."23:00" en day, "yyyy-MM-dd" en week/month.</summary>
    public string Etiqueta { get; set; } = string.Empty;

    public DateTime Inicio { get; set; }

    public decimal TotalM3 { get; set; }
}

/// <summary>HU-016: respuesta de la consulta de consumo agregado por período.</summary>
public class ConsumoConsultaResponseDto
{
    public int HogarId { get; set; }

    /// <summary>"day" | "week" | "month"</summary>
    public string Periodo { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public decimal TotalM3 { get; set; }

    public decimal PromedioM3 { get; set; }

    public List<ConsumoPuntoDto> Datos { get; set; } = new();
}
