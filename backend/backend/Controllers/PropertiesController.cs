using backend.DTOs;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _svc;
    public PropertiesController(IPropertyService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetAll()
        => Ok(await _svc.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<PropertyResponseDto>> Create(CreatePropertyDto dto)
    {
        try
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
