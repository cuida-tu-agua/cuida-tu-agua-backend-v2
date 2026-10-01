using System.Net;
using System.Text.Json;
using CuidaTuAguaBackend.Common.Exceptions;

namespace CuidaTuAguaBackend.Common.Middleware;

/// <summary>
/// Traduce las excepciones de dominio a respuestas HTTP consistentes en formato
/// ProblemDetails, para que el frontend siempre reciba { status, title, detail }.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.Conflict, "Conflicto", ex.Message);
        }
        catch (BadRequestException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, "Solicitud inválida", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Error interno",
                "Ocurrió un error inesperado. Intenta de nuevo más tarde.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            status = (int)statusCode,
            title,
            detail,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
