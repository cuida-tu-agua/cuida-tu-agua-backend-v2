namespace CuidaTuAguaBackend.Common.Exceptions;

/// <summary>Se lanza cuando la operación choca con un recurso existente (ej. dirección/teléfono duplicados). Se traduce a HTTP 409.</summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
