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

    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly AppDbContext _context;

        public EventosController(ILogger<EventosController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/eventos")]
        // [MapToApiVersion("2.0")]
        public IActionResult ListarEventos()
        {
            var eventos = _context.Eventos.ToList();

            if (eventos.Count == 0) return NotFound("Eventos não localizados");

            return Ok(eventos);
        }


        [HttpGet("/eventos/abertos")]
        [AllowAnonymous]
        public IActionResult ListarEventosAbertos()
        {
            var eventos = _context.Eventos.Where(x => x.TipoEvento == API.Enums.ETipoEvento.Aberto).ToList();

            if (eventos.Count == 0) return NotFound("Eventos não localizados");

            return Ok(eventos);
        }

        [HttpGet("/eventos/{id:int}")]
        [MapToApiVersion("1.0")]
        public IActionResult ListarProdutosPorId(int id)
        {
            var produtosEntity = _context.Eventos.FirstOrDefault(x => x.Id == id);

            if (produtosEntity is null) return NotFound("Eventos n�o localizado");

            return Ok(produtosEntity);
        }

        [HttpPost("/eventos")]
        public async Task<IActionResult> SalvarEvento(EventoDto evento)
        {
            var eventoEntity = new EventosEntity
            {
                Data = evento.Data,
                Descricao = evento.Descricao,
                TipoEvento = evento.TipoEvento,
                Horario = evento.Horario,
                Detalhes = evento.Detalhes
            };
            _context.Eventos.Add(eventoEntity);
            await _context.SaveChangesAsync();

            return Created($"/eventos/{eventoEntity.Id}", eventoEntity);
        }

        [HttpPut("/eventos/{id}")]
        public async Task<IActionResult> AtualizarEvento(int id, EventoDto evento)
        {
            var pedidoEntity = _context.Eventos.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (pedidoEntity is null) return NotFound("Evento n�o localizado");

            pedidoEntity.Data = evento.Data;
            pedidoEntity.Descricao = evento.Descricao;
            pedidoEntity.TipoEvento = evento.TipoEvento;
            pedidoEntity.Horario = evento.Horario;
            pedidoEntity.Detalhes = evento.Detalhes;

            _context.Eventos.Update(pedidoEntity);
            await _context.SaveChangesAsync();

            return Ok(pedidoEntity);
        }


        [HttpDelete("/eventos/{id}")]
        public async Task<IActionResult> ExcluirEvento(int id)
        {
            var eventosEntity = _context.Eventos.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (eventosEntity is null) return NotFound("Evento n�o localizado");

            _context.Eventos.Remove(eventosEntity);
            await _context.SaveChangesAsync();

            return Ok("Evento exclu�do com sucesso");
        }
    }
}
