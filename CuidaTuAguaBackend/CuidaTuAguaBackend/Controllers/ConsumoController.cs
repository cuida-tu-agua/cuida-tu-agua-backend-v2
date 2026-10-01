using CuidaTuAguaBackend.DTOs;
using CuidaTuAguaBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CuidaTuAguaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumoController : ControllerBase
{
    private readonly IConsumoService _consumoService;

    public ConsumoController(IConsumoService consumoService)
    {
        _consumoService = consumoService;
    }

    /// <summary>
    /// Registra una lectura de consumo de agua para un hogar.
    /// No está pedido explícitamente por HU-016, pero es necesario para poder
    /// cargar datos de prueba y probar el endpoint de consulta con datos reales.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ConsumoResponseDto>> Registrar([FromBody] ConsumoCreateDto dto)
    {
        var creado = await _consumoService.RegistrarAsync(dto);
        return StatusCode(201, creado);
    }

    /// <summary>
    /// HU-016: consulta el consumo de un hogar agregado por período.
    /// Ejemplos:
    ///   GET /api/consumo?hogarId=1&amp;periodo=day&amp;fecha=2026-09-30
    ///     -> 24 puntos, uno por hora de ese día.
    ///   GET /api/consumo?hogarId=1&amp;periodo=week&amp;fecha=2026-09-30
    ///     -> 7 puntos, uno por día de esa semana (lunes a domingo).
    ///   GET /api/consumo?hogarId=1&amp;periodo=month&amp;fecha=2026-09-30
    ///     -> un punto por cada día de ese mes.
    /// Si no se manda "fecha", se usa el día de hoy (UTC) como referencia.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ConsumoConsultaResponseDto>> Consultar(
        [FromQuery] int hogarId,
        [FromQuery] string periodo = "day",
        [FromQuery] DateTime? fecha = null)
    {
        var resultado = await _consumoService.ConsultarAsync(hogarId, periodo, fecha);
        return Ok(resultado);
    }
}
