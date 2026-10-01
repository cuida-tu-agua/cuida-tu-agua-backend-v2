namespace CuidaTuAguaBackend.Models;

/// <summary>
/// Representa una lectura individual de consumo de agua de un hogar
/// (HU-016). Un hogar tiene muchas lecturas a lo largo del tiempo.
/// </summary>
public class ConsumoAgua
{
    public int Id { get; set; }

    public int HogarId { get; set; }

    public Hogar? Hogar { get; set; }

    /// <summary>Fecha y hora (UTC) en la que se registró la lectura.</summary>
    public DateTime FechaHora { get; set; }

    /// <summary>Consumo registrado en esa lectura, en metros cúbicos (m3).</summary>
    public decimal ValorM3 { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
