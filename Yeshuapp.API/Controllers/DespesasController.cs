
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Dtos;
using Yeshuapp.Entities;

namespace Yeshuapp.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DespesasController : ControllerBase
    {
        private readonly ILogger<DespesasController> _logger;
        private readonly AppDbContext _context;

        public DespesasController(ILogger<DespesasController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/despesas")]
        public IActionResult ListarDespesas()
        {
            var result = _context.Despesas.ToList();

            if (result.Count == 0) return NotFound("Despesas nao localizadas");

            return Ok(result);
        }

        [HttpGet("/despesas/{id:int}")]
        public IActionResult ListarDespesaPorId(int id)
        {
            var despesaEntity = _context.Despesas.FirstOrDefault(x => x.Id == id);

            if (despesaEntity is null) return NotFound("Despesa nao localizada");

            return Ok(despesaEntity);
        }

        [HttpPost("/despesas")]
        public async Task<IActionResult> SalvarDespesas(DespesasDto despesaDto)
        {
            var despesasEntity = new DespesasEntity
            {
                Descricao = despesaDto.Descricao,
                Data = DateTime.SpecifyKind(despesaDto.Data, DateTimeKind.Utc),
                Detalhes = despesaDto.Detalhes,
                Valor = despesaDto.Valor,
                Anexo = despesaDto.Anexo
            };
            _context.Despesas.Add(despesasEntity);
            await _context.SaveChangesAsync();

            var fluxoCaixaEntity = new FluxoCaixaEntity
            {
                Tipo = "Saida",
                Data = DateTime.SpecifyKind(despesasEntity.Data, DateTimeKind.Utc),
                Valor = despesasEntity.Valor,
                Origem = "Gasto da Igreja",
                Descricao = $"Despesa #{despesasEntity.Id} - Descricao: #{despesasEntity.Descricao}"
            };

            await _context.FluxoCaixa.AddAsync(fluxoCaixaEntity);
            await _context.SaveChangesAsync();

            return Created($"/despesas/{despesasEntity.Id}", despesasEntity);
        }

        [HttpPut("/despesas/{id}")]
        public async Task<IActionResult> AtualizarDespesa(int id, DespesasResponseDto despesaUpdateDto)
        {
            var despesaEntity = _context.Despesas.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (despesaEntity is null) return NotFound("Despesa nao localizada");

            despesaEntity.Descricao = despesaUpdateDto.Descricao;
            despesaEntity.Data = despesaUpdateDto.Data;
            despesaEntity.Detalhes = despesaUpdateDto.Detalhes;
            despesaEntity.Anexo = despesaUpdateDto.Anexo;
            despesaEntity.Valor = despesaUpdateDto.Valor;

            _context.Despesas.Update(despesaEntity);
            await _context.SaveChangesAsync();

            var fluxoCaixaEntity = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == id);

            fluxoCaixaEntity.Valor = despesaEntity.Valor;
            fluxoCaixaEntity.Data = despesaEntity.Data;
            fluxoCaixaEntity.Descricao = $"Despesa #{despesaEntity.Id} - Descricao: #{despesaEntity.Descricao}";
            _context.FluxoCaixa.Update(fluxoCaixaEntity);
            await _context.SaveChangesAsync();

            return Ok(despesaEntity);
        }


        [HttpDelete("/despesas/{id}")]
        public async Task<IActionResult> ExcluirDespesa(int id)
        {
            var despesasEntity = _context.Despesas.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (despesasEntity is null) return NotFound("Despesa nao localizada");

            _context.Despesas.Remove(despesasEntity);
            await _context.SaveChangesAsync();

            var fluxoCaixaEntity = await _context.FluxoCaixa.FirstOrDefaultAsync(x => x.Id == id);

            if (fluxoCaixaEntity is not null)
            {
                _context.FluxoCaixa.Remove(fluxoCaixaEntity);
                await _context.SaveChangesAsync();
            }

            return Ok("Despesa excluida com sucesso");
        }
    }
}
