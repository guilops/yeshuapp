using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Dtos;
using Yeshuapp.Entities;

namespace Yeshuapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FrasesController : ControllerBase
    {
        private readonly ILogger<FrasesController> _logger;
        private readonly AppDbContext _context;

        public FrasesController(ILogger<FrasesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/frases")]
        public IActionResult ListarFrases()
        {
            var frases = _context.Frases.ToList();

            if (frases.Count == 0) return NotFound("Frases n�o localizadas");

            return Ok(frases);
        }

        [HttpGet("/frases/{id:int}")]
        public IActionResult ListarFrasesPorId(int id)
        {
            var fraseEntity = _context.Frases.FirstOrDefault(x => x.Id == id);

            if (fraseEntity is null) return NotFound("Frase n�o localizada");

            return Ok(fraseEntity);
        }


        [HttpPost("/frases/lista")]
        public async Task<IActionResult> SalvarFrases(List<FraseDto> frases)
        {
            foreach (var frase in frases)
            {
                var fraseEntity = new FrasesEntity
                {
                    Versiculo = frase.Versiculo,
                    Capitulo = frase.Capitulo,
                    Livro = frase.Livro,
                    Passagem = frase.Passagem,
                    Ativa = true
                };
                _context.Frases.Add(fraseEntity);
                await _context.SaveChangesAsync();
            }

            return Ok("Frases salvas com sucesso");
        }

        [HttpPost("/frases")]
        public async Task<IActionResult> SalvarFrase(FraseDto frase)
        {
            var fraseEntity = new FrasesEntity
            {
                Versiculo = frase.Versiculo,
                Capitulo = frase.Capitulo,
                Livro = frase.Livro,
                Passagem = frase.Passagem,
                Ativa = true
            };
            _context.Frases.Add(fraseEntity);
            await _context.SaveChangesAsync();

            return Created($"/frases/{fraseEntity.Id}", fraseEntity);
        }

        [HttpPut("/frases/{id}")]
        public async Task<IActionResult> AtualizarFrase(int id, FraseDto frase)
        {
            var fraseEntity = _context.Frases.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (fraseEntity is null) return NotFound("Frase n�o localizada");

            fraseEntity.Passagem = frase.Passagem;
            fraseEntity.Livro = frase.Livro;
            fraseEntity.Ativa = frase.Ativa;
            fraseEntity.Capitulo = frase.Capitulo;
            fraseEntity.Versiculo = frase.Versiculo;

            _context.Frases.Update(fraseEntity);
            await _context.SaveChangesAsync();

            return Ok(fraseEntity);
        }


        [HttpDelete("/frases/{id}")]
        public async Task<IActionResult> ExcluirFrase(int id)
        {
            var fraseEntity = _context.Frases.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (fraseEntity is null) return NotFound("Frase n�o localizada");

            _context.Frases.Remove(fraseEntity);
            await _context.SaveChangesAsync();

            return Ok("Frase exclu�do com sucesso");
        }
    }
}
