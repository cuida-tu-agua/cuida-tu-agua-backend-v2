namespace CuidaTuAguaBackend.Common.Exceptions;

/// <summary>Se lanza cuando un recurso solicitado no existe. Se traduce a HTTP 404.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
