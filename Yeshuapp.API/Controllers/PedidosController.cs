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
                        Quantidade = pp.Quantidade,
                        Produto = new ProdutoDto
                        {
                            Valor = pp.Produto.Valor,
                            Nome = pp.Produto.Nome,
                            Quantidade = pp.Produto.Quantidade
                        }

                    }).ToList(),
                    Cliente = new ClienteDto
                    {
                        Nome = p.Cliente.Nome,
                        TelefoneCelular = p.Cliente.TelefoneCelular,
                        Email = p.Cliente.Email,
                    }
                }).Where(s=> s.StatusPedido == API.Enums.EStatusPedido.Aberto).ToListAsync();

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
                        Quantidade = pp.Quantidade,
                        Produto = new ProdutoDto
                        {
                            Valor = pp.Produto.Valor,
                            Nome = pp.Produto.Nome,
                            Quantidade = pp.Produto.Quantidade
                        }

                    }).ToList(),
                    Cliente = new ClienteDto
                    {
                        Nome = p.Cliente.Nome,
                        TelefoneCelular = p.Cliente.TelefoneCelular,
                        Email = p.Cliente.Email,
                    }
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
            if (clienteEntity == null) 
                return NotFound("Cliente informado não localizado");

            decimal valorProdutosSomados = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var produtoPedido in pedido.Produtos)
                {
                    var produtoExistente = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == produtoPedido.Id);

                    if (produtoExistente == null)
                        return NotFound($"Produto com ID {produtoPedido.Id} não encontrado.");

                    if (produtoExistente.Quantidade < produtoPedido.Quantidade)
                        return BadRequest($"Estoque insuficiente para o produto {produtoExistente.Nome}.");

                    produtoExistente.Quantidade -= produtoPedido.Quantidade;

                    _context.Produtos.Update(produtoExistente);

                    valorProdutosSomados += produtoPedido.Quantidade * produtoExistente.Valor;
                }

                if (Math.Round(valorProdutosSomados, 2) != Math.Round(pedido.Valor, 2))
                    return BadRequest("Valor do Pedido diferente da soma dos produtos");

                var pedidoEntity = new PedidosEntity
                {
                    Cliente = clienteEntity,
                    StatusPedido = API.Enums.EStatusPedido.Aberto,
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

                await transaction.CommitAsync();

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
                            Quantidade = pp.Quantidade,
                            Produto = new ProdutoDto
                            {
                                Valor = pp.Produto.Valor,
                                Nome = pp.Produto.Nome,
                                Quantidade = pp.Produto.Quantidade
                            }
                        }).ToList(),
                        Cliente = new ClienteDto
                        {
                            Nome = p.Cliente.Nome,
                            TelefoneCelular = p.Cliente.TelefoneCelular,
                            Email = p.Cliente.Email,
                        }
                    })
                    .FirstOrDefaultAsync();

                if (pedidoConsulta == null)
                    return StatusCode(500,"Não foi possível cadastrar o pedido");

                var fluxoCaixaEntity = new FluxoCaixaEntity
                {
                    Tipo = "Entrada",
                    Valor = pedidoEntity.Valor,
                    Origem = "Venda de Produtos",
                    Descricao = $"Pedido #{pedidoEntity.Id} - Cliente: #{pedidoEntity.Cliente.Nome}"
                };

                await _context.FluxoCaixa.AddAsync(fluxoCaixaEntity);
                await _context.SaveChangesAsync();

                return Created($"/pedidos/{pedidoConsulta.Id}", pedidoConsulta);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao salvar pedido: {ex.Message}");
            }
        }

        [HttpPut("/pedidos/{id:int}")]
        public async Task<IActionResult> EditarPedido(int id, PedidoDto pedido)
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
                    return BadRequest("Estoque insuficiente para o produto solicitado.");

                produtoExistente.Quantidade -= produtoPedido.Quantidade;

                valorProdutosSomados += pedido.Produtos.Where(x => x.Id == produtoExistente.Id).Sum(x => x.Quantidade * produtoExistente.Valor);

            }

            if (Math.Round(valorProdutosSomados, 2) != Math.Round(pedido.Valor, 2))
                return BadRequest("Valor do Pedido diferente da soma dos produtos incluídos.");

            var pedidoEntity = new PedidosEntity
            {
                Id = id,
                Cliente = clienteEntity,
                Data = pedido.Data,
                StatusPedido = pedido.StatusPedido,
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
                        Quantidade = pp.Quantidade,
                        Produto = new ProdutoDto
                        {
                            Valor = pp.Produto.Valor,
                            Nome = pp.Produto.Nome,
                            Quantidade = pp.Produto.Quantidade
                        }
                        
                    }).ToList(),
                    Cliente = new ClienteDto
                    {
                        Nome = p.Cliente.Nome,
                        TelefoneCelular = p.Cliente.TelefoneCelular,
                        Email = p.Cliente.Email,
                    }
                })
                .FirstOrDefaultAsync();

            if (pedidoConsulta == null)
                return StatusCode(500, "Não foi possivel atualizar o pedido");

            // Registrar movimentação no Fluxo de Caixa
            var fluxoCaixaEntity = new FluxoCaixaEntity
            {
                Data = DateTime.Now,
                Tipo = "Entrada",
                Valor = pedidoEntity.Valor,
                Origem = "Venda de Produtos",
                Descricao = $"Pedido #{pedidoEntity.Id} - Cliente: #{pedidoEntity.Cliente.Nome}"
            };

            await _context.FluxoCaixa.AddAsync(fluxoCaixaEntity);
            await _context.SaveChangesAsync();

            return Ok(pedidoConsulta);

        }

        [HttpDelete("/pedidos/{id:int}")]
        public async Task<IActionResult> DeletarPedido(int id)
        {
            var pedidoEntity = _context.Pedidos.FirstOrDefault(x => x.Id == id);
            if (pedidoEntity == null) return NotFound("Pedido informado não localizado");

            pedidoEntity.StatusPedido = API.Enums.EStatusPedido.Fechado;

            _context.Pedidos.Update(pedidoEntity);
            await _context.SaveChangesAsync();

            return Ok("Pedido encerrado com sucesso");

        }
    }
}
