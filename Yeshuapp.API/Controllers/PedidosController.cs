using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yeshuapp.Context;
using Yeshuapp.Dtos;
using Yeshuapp.Entities;

namespace Yeshuapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly AppDbContext _context;

        public PedidosController(ILogger<PedidosController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/pedidos")]
        public async Task<IActionResult> ListarPedidos()
        {
            var pedidos = await _context.Pedidos
               .Include(p => p.PedidoProdutos)
               .ThenInclude(pp => pp.Produto)
               .Include(p => p.Cliente)
               .Select(p => new PedidoResponseDto
               {
                   Id = p.Id,
                   CodigoCliente = p.Cliente.Id,
                   Data = p.Data,
                   StatusPedido = p.StatusPedido,
                   Valor = p.Valor,
                   Produtos = p.PedidoProdutos.Select(pp => new ProdutoPedidoDto
                   {
                       Id = pp.ProdutoId,
                       Quantidade = pp.Quantidade
                   }).ToList()
               }).ToListAsync();

            if (!pedidos.Any()) return NotFound();

            return Ok(pedidos);
        }

        [HttpGet("/pedidos/{id:int}")]
        public async Task<IActionResult> ListarPedidoPorId(int id)
        {
            var pedidoConsulta = await _context.Pedidos
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .Include(p => p.Cliente)
                .Where(p => p.Id == id)
                .Select(p => new PedidoResponseDto
                {
                    Id = p.Id,
                    CodigoCliente = p.Cliente.Id,
                    Data = p.Data,
                    StatusPedido = p.StatusPedido,
                    Valor = p.Valor,
                    Produtos = p.PedidoProdutos.Select(pp => new ProdutoPedidoDto
                    {
                        Id = pp.ProdutoId,
                        Quantidade = pp.Quantidade
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (pedidoConsulta == null)
                return NotFound("Pedido informado não localizado");

            return Ok(pedidoConsulta);
        }

        [HttpPost("/pedidos")]
        public async Task<IActionResult> SalvarPedido(PedidoDto pedido)
        {
            var clienteEntity = _context.Clientes.FirstOrDefault(x => x.Id == pedido.CodigoCliente);
            if (clienteEntity == null) return NotFound("Cliente informado não localizado");

            decimal valorProdutosSomados = 0;

            foreach (var produtoPedido in pedido.Produtos)
            {
                var produtoExistente = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produtoPedido.Id);

                if (produtoExistente == null)
                    return NotFound($"Produto com ID {produtoPedido.Id} não encontrado.");

                if (produtoExistente.Quantidade < produtoPedido.Quantidade)
                    return BadRequest("Estoque insuficiente para o produto.");

                produtoExistente.Quantidade -= produtoPedido.Quantidade;

                valorProdutosSomados += pedido.Produtos.Where(x=> x.Id == produtoExistente.Id).Sum(x => x.Quantidade * produtoExistente.Valor);

            }

            if (Math.Round(valorProdutosSomados, 2) != Math.Round(pedido.Valor, 2))
                return BadRequest("Valor do Pedido diferente da soma dos produtos");

            var pedidoEntity = new PedidosEntity
            {
                Cliente = clienteEntity,
                Data = pedido.Data,
                StatusPedido = true,
                Valor = pedido.Valor,
                PedidoProdutos = pedido.Produtos
                    .Select(dto => new PedidoProdutoEntity
                    {
                        ProdutoId = dto.Id,
                        Quantidade = dto.Quantidade
                    })
                    .ToList()
            };

            _context.Pedidos.Add(pedidoEntity);
            await _context.SaveChangesAsync();


            var pedidoConsulta = await _context.Pedidos
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .Include(p => p.Cliente)
                .Where(p => p.Id == pedidoEntity.Id)
                .Select(p => new PedidoResponseDto
                {
                    Id = p.Id,
                    CodigoCliente = p.Cliente.Id,
                    Data = p.Data,
                    StatusPedido = p.StatusPedido,
                    Valor = p.Valor,
                    Produtos = p.PedidoProdutos.Select(pp => new ProdutoPedidoDto
                    {
                        Id = pp.ProdutoId,
                        Quantidade = pp.Quantidade
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (pedidoConsulta == null)
                return StatusCode(500,"Não foi possivel cadastrar o pedido");

            return Created($"/pedidos/{pedidoConsulta.Id}", pedidoConsulta);

        }
    }
}
