using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Entities;

namespace Yeshuapp.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class PedidoOracaoController : ControllerBase
{
    private readonly ILogger<PedidoOracaoController> _logger;
    private readonly AppDbContext _context;
    public PedidoOracaoController(ILogger<PedidoOracaoController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("/pedido-oracao")]
    public async Task<IActionResult> Create([FromBody] PedidoOracaoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = new PedidoOracaoEntity
        {
            Id = Guid.NewGuid(),
            Message = dto.Mensagem,
            CreatedAt = DateTime.UtcNow,
            Status = "1"
        };

        await _context.PedidoOracao.AddAsync(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaged), new { page = 1, pageSize = 10 }, entity);
    }

    [HttpGet("/pedido-oracao")]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var result = await _context.PedidoOracao.ToListAsync(ct);

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }
}