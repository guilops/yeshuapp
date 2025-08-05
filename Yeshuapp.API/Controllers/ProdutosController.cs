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
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> _logger;
        private readonly AppDbContext _context;

        public ProdutosController(ILogger<ProdutosController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/produtos")]
        public IActionResult ListarProdutos()
        {
            var result = _context.Produtos.ToList();

            if (result.Count == 0) return NotFound("Produtos n�o localizados");

            return Ok(result);
        }

        [HttpGet("/produtos/{id:int}")]
        public IActionResult ListarProdutosPorId(int id)
        {
            var produtosEntity = _context.Produtos.FirstOrDefault(x=> x.Id == id);

            if (produtosEntity is null) return NotFound("Produto n�o localizado");

            return Ok(produtosEntity);
        }

        [HttpPost("/produtos")]
        public async Task<IActionResult> SalvarProduto(ProdutoDto produto)
        {
            var produtoEntity = new ProdutosEntity
            {
                Nome = produto.Nome,
                Quantidade = produto.Quantidade,
                Valor = produto.Valor,
                Imagem = produto.Imagem
            };
            _context.Produtos.Add(produtoEntity);
            await _context.SaveChangesAsync();

            return Created($"/produtos/{produtoEntity.Id}", produtoEntity);
        }

        [HttpPut("/produtos/{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, ProdutoDto produto)
        {
            var produtoEntity = _context.Produtos.FirstOrDefaultAsync(x=> x.Id==id).Result;    

            if (produtoEntity is null) return NotFound("Produto n�o localizado");

            produtoEntity.Nome = produto.Nome;
            produtoEntity.Quantidade = produto.Quantidade;
            produtoEntity.Valor = produto.Valor;
           
            _context.Produtos.Update(produtoEntity);
            await _context.SaveChangesAsync();

            return Ok(produtoEntity);
        }


        [HttpDelete("/produtos/{id}")]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var produtoEntity = _context.Produtos.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (produtoEntity is null) return NotFound("Produto n�o localizado");

            var pedidosExistentes = await _context.Pedidos.AnyAsync(p => p.Cliente.Id == id && p.StatusPedido == API.Enums.EStatusPedido.Aberto);

            if (pedidosExistentes)
                return BadRequest("N�o � poss�vel excluir o produto, pois existem pedidos ativos associados a ele.");

            _context.Produtos.Remove(produtoEntity);
            await _context.SaveChangesAsync();

            return Ok("Produto exclu�do com sucesso");
        }
    }
}
