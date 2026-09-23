using System.ComponentModel.DataAnnotations;

namespace CuidaTuAguaBackend.DTOs;

/// <summary>
/// HU-008 - Datos requeridos para registrar un hogar.
/// Las validaciones reflejan las que ya existen en el formulario del frontend
/// (RegisterScreen.tsx / register.json).
/// </summary>
public class HogarCreateDto
{
    [Required(ErrorMessage = "El nombre del hogar es obligatorio")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 250 caracteres")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^[0-9+\s\-]{10,15}$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(100)]
    public string Pais { get; set; } = "Colombia";

    [Required(ErrorMessage = "El estrato es obligatorio")]
    [StringLength(5)]
    public string Estrato { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de habitantes es obligatorio")]
    [Range(1, 50, ErrorMessage = "El número de habitantes debe ser mayor a 0")]
    public int Habitantes { get; set; }

    /// <summary>Opcional: sin tabla de Usuarios todavía, se acepta pero no se valida contra nada.</summary>
    public int? UsuarioId { get; set; }
}

/// <summary>
/// HU-009 - Datos requeridos para editar un hogar existente.
/// Es un reemplazo completo del recurso (PUT), por eso los campos son obligatorios.
/// </summary>
public class HogarUpdateDto
{
    [Required(ErrorMessage = "El nombre del hogar es obligatorio")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 250 caracteres")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^[0-9+\s\-]{10,15}$", ErrorMessage = "El teléfono debe tener entre 10 y 15 dígitos")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio")]
    [StringLength(100)]
    public string Pais { get; set; } = string.Empty;

    [Required(ErrorMessage = "El estrato es obligatorio")]
    [StringLength(5)]
    public string Estrato { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de habitantes es obligatorio")]
    [Range(1, 50, ErrorMessage = "El número de habitantes debe ser mayor a 0")]
    public int Habitantes { get; set; }

    public int? UsuarioId { get; set; }
}

/// <summary>Forma en la que se devuelve un Hogar al cliente.</summary>
public class HogarResponseDto
{
    public int Id { get; set; }
    public int? UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string Estrato { get; set; } = string.Empty;
    public int Habitantes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
