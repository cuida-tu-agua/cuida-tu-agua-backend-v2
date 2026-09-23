using CuidaTuAguaBackend.DTOs;
using CuidaTuAguaBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CuidaTuAguaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HogaresController : ControllerBase
{
    private readonly IHogarService _hogarService;

    public HogaresController(IHogarService hogarService)
    {
        _hogarService = hogarService;
    }

    /// <summary>Lista todos los hogares. Útil para poblar pantallas de listado/edición.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HogarResponseDto>>> GetAll()
    {
        var hogares = await _hogarService.ObtenerTodosAsync();
        return Ok(hogares);
    }

    /// <summary>Obtiene un hogar por id. Usado por la pantalla de Editar para precargar el formulario.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<HogarResponseDto>> GetById(int id)
    {
        var hogar = await _hogarService.ObtenerPorIdAsync(id);
        return Ok(hogar);
    }

    /// <summary>HU-008: Registrar un lugar.</summary>
    [HttpPost]
    public async Task<ActionResult<HogarResponseDto>> Create([FromBody] HogarCreateDto dto)
    {
        var creado = await _hogarService.RegistrarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
    }

    /// <summary>HU-009: Editar lugar.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<HogarResponseDto>> Update(int id, [FromBody] HogarUpdateDto dto)
    {
        var actualizado = await _hogarService.EditarAsync(id, dto);
        return Ok(actualizado);
    }
}
