using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Entities;
using Yeshuapp.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Yeshuapp.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class VisitantesController : ControllerBase
{
    private readonly ILogger<VisitantesController> _logger;
    private readonly AppDbContext _context;
    public VisitantesController(ILogger<VisitantesController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("/visitantes")]
    public async Task<IActionResult> Create([FromBody] VisitanteDto dto)
    {
        var entity = new VisitanteEntity
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            VisitDate = DateTime.UtcNow,
            Notes = dto.Notes,
            WantsContact = dto.WantsContact
        };

        await _context.Visitante.AddAsync(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaged), new { page = 1, pageSize = 10 }, entity);
    }

    [HttpGet("/visitantes")]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var result = await _context.Visitante.ToListAsync(ct);

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }
}