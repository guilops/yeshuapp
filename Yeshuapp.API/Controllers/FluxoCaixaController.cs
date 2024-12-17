using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Dtos;

[ApiController]
[Route("[controller]")]
public class FluxoCaixaController : ControllerBase
{
    private readonly AppDbContext _context;

    public FluxoCaixaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("/fluxocaixa/relatorio")]
    public async Task<IActionResult> ObterRelatorioFluxoCaixa(
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] string tipo // "Entrada", "Saída" ou nulo para todos
    )
    {
        // Base da query
        var query = _context.FluxoCaixa.AsQueryable();

        // Aplicar filtros de data
        if (dataInicio.HasValue)
            query = query.Where(fc => fc.Data >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(fc => fc.Data <= dataFim.Value);

        // Filtrar por tipo, se informado
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(fc => fc.Tipo == tipo);

        // Obter resultados
        var resultado = await query
            .OrderBy(fc => fc.Data)
            .Select(fc => new FluxoCaixaDto
            {
                Data = fc.Data,
                Tipo = fc.Tipo,
                Valor = fc.Valor,
                Origem = fc.Origem,
                Descricao = fc.Descricao
            })
            .ToListAsync();

        // Calcular totais
        var totalEntrada = resultado.Where(fc => fc.Tipo == "Entrada").Sum(fc => fc.Valor);
        var totalSaida = resultado.Where(fc => fc.Tipo == "Saída").Sum(fc => fc.Valor);

        var resumo = new
        {
            TotalEntrada = totalEntrada,
            TotalSaida = totalSaida,
            Saldo = totalEntrada - totalSaida,
            Itens = resultado
        };

        return Ok(resumo);
    }
}