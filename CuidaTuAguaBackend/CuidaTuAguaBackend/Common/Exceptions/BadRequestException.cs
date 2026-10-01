namespace CuidaTuAguaBackend.Common.Exceptions;

/// <summary>Se lanza cuando el request tiene datos inválidos que no detecta el model binding (ej. "periodo=invalido"). Se traduce a HTTP 400.</summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
