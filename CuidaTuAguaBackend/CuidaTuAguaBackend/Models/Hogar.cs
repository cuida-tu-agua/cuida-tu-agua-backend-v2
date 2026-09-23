namespace CuidaTuAguaBackend.Models;

/// <summary>
/// Representa un hogar registrado en la plataforma (HU-008 / HU-009).
/// </summary>
public class Hogar
{
    public int Id { get; set; }

    /// <summary>
    /// FK opcional al usuario dueño del hogar. Aún no existe tabla de Usuarios
    /// en el backend, por lo que se deja como columna nullable sin restricción
    /// de llave foránea. Cuando exista la entidad Usuario, agregar la FK real
    /// y una migración que la vincule.
    /// </summary>
    public int? UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Pais { get; set; } = string.Empty;

    /// <summary>
    /// Se maneja como texto porque el estrato varía por país
    /// (numérico en Colombia: "1"-"6", alfabético en Ecuador: "A", "B", "C+", "C-", "D").
    /// </summary>
    public string Estrato { get; set; } = string.Empty;

    public int Habitantes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
