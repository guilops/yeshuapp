using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Dtos;
using Yeshuapp.Entities;

namespace Yeshuapp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class IrmaosController : ControllerBase
    {
        private readonly ILogger<IrmaosController> _logger;
        private readonly AppDbContext _context;

        public IrmaosController(ILogger<IrmaosController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/irmaos")]
        public IActionResult ListarClientes()
        {
            var result = _context.Clientes.ToList();

            if (result.Count == 0) return NotFound("Nenhum irm�o foi localizado");

            return Ok(result);
        }

        [HttpGet("/irmaos/{id:int}")]
        public IActionResult ListarClientesPorId(int id)
        {
            var clienteEntity = _context.Clientes.FirstOrDefault(x => x.Id == id);

            if (clienteEntity is null) return NotFound("Irm�o n�o localizado");

            return Ok(clienteEntity);
        }

        [HttpPost("/irmaos")]
        public async Task<IActionResult> SalvarClientes(ClienteDto cliente)
        {
            var clienteEntity = new ClientesEntity
            {
                CPF = cliente.CPF,
                Email = cliente.Email,
                Nome = cliente.Nome,
                TelefoneCelular = cliente.TelefoneCelular,
                TelefoneFixo = cliente.TelefoneFixo,
                Sexo = cliente.Sexo,
                DataNascimento = cliente.DataNascimento,
                Imagem = cliente.Imagem
            };

            _context.Clientes.Add(clienteEntity);
            await _context.SaveChangesAsync();

            return Created($"/irmaos/{clienteEntity.Id}", clienteEntity);
        }

        [HttpPut("/irmaos/{id}")]
        public async Task<IActionResult> AtualizarCliente(int id, ClienteDto clienteDto)
        {
            var clienteEntity = _context.Clientes.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (clienteEntity is null) return NotFound("Irm�o n�o localizado");

            clienteEntity.Nome = clienteDto.Nome;
            clienteEntity.CPF = clienteDto.CPF;
            clienteEntity.Email = clienteDto.Email;
            clienteEntity.TelefoneCelular = clienteDto.TelefoneCelular;
            clienteEntity.TelefoneFixo = clienteDto.TelefoneFixo;
            clienteEntity.Sexo = clienteDto.Sexo;
            clienteEntity.DataNascimento = clienteDto.DataNascimento;
            clienteEntity.Imagem = clienteDto.Imagem;

            _context.Clientes.Update(clienteEntity);
            await _context.SaveChangesAsync();

            return Ok(clienteEntity);
        }


        [HttpDelete("/irmaos/{id}")]
        public async Task<IActionResult> ExcluirClientes(int id)
        {
            var clienteEntity = _context.Clientes.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (clienteEntity is null) return NotFound("Irm�o n�o localizado");

            var pedidosExistentes = await _context.Pedidos.AnyAsync(p => p.Cliente.Id == id && p.StatusPedido == API.Enums.EStatusPedido.Aberto);

            if (pedidosExistentes)
                return BadRequest("N�o � poss�vel excluir o irm�o, pois existem pedidos em aberto associados a ele.");

            _context.Clientes.Remove(clienteEntity);
            await _context.SaveChangesAsync();

            return Ok("Irm�o exclu�do com sucesso");
        }
    }
}
