using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.WebSockets;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public PedidoResponseDto? Pedido { get; set; } = new PedidoResponseDto();
        public List<SelectListItem> Irmaos { get; set; }
        private readonly PedidosServices _pedidosServices;
        private readonly ProdutosServices _produtosServices;
        private readonly IrmaosServices _irmaosServices;
        public string ErrorMessage;

        public EditModel(PedidosServices pedidosServices, ProdutosServices produtosServices, IrmaosServices irmaosServices)
        {
            _pedidosServices = pedidosServices;
            _produtosServices = produtosServices;
            _irmaosServices = irmaosServices;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _pedidosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            _produtosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            _irmaosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

            var resultPedido = await _pedidosServices.GetPedidoByIdAsync(id);
            var resultProdutos = await _produtosServices.GetProdutosAsync();

            if (!resultPedido.IsSuccessStatusCode)
            {
                ErrorMessage = "N�o foi poss�vel carregar as informa��es necess�rias";
                return Page();
            }

            Pedido = await resultPedido.Content.ReadFromJsonAsync<PedidoResponseDto>();

            if (Pedido == null)
            {
                return NotFound();
            }

            var produtos = await resultProdutos.Content.ReadFromJsonAsync<List<ProdutoDto>>();

            foreach (var produtoPedido in Pedido.Produtos)
            {
                var produtoDetalhado = produtos.FirstOrDefault(x => x.Id == produtoPedido.Id);

                produtoPedido.Produto = produtoDetalhado;
            }

            var resultIrmaos = await _irmaosServices.GetIrmaoByIdAsync(Pedido.CodigoCliente);

            var irmao = await resultIrmaos.Content.ReadFromJsonAsync<ClienteResponseDto>();

            Irmaos = new List<SelectListItem>
            {
                new SelectListItem
                {
                Value = irmao.Id.ToString(),
                Text = irmao.Nome
                }
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _pedidosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            Pedido.Data = DateTime.Now;
            Pedido.StatusPedido = Enums.EStatusPedido.Aberto;
            Pedido.Cliente = await (await _irmaosServices.GetIrmaoByIdAsync(Pedido.CodigoCliente)).Content.ReadFromJsonAsync<ClienteRequestDto>();

            var resultProdutos = await _produtosServices.GetProdutosAsync();

            var produtos = await resultProdutos.Content.ReadFromJsonAsync<List<ProdutoDto>>();

            foreach (var produtoPedido in Pedido.Produtos)
            {
                var produto = Pedido.Produtos?.FirstOrDefault(x => x.Id == produtoPedido.Id);

                produto.Produto = produtos.FirstOrDefault(x=> x.Id == produtoPedido.Id);
                produto.Produto.Quantidade = produto.Produto.Quantidade - produtoPedido.Quantidade;

            }

            Pedido.Valor = Pedido.Produtos.Sum(p => p.Produto.Valor * p.Quantidade);

            var response = await _pedidosServices.UpdatePedidoAsync(Pedido);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Pedidos/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar o pedido.");
            return Page();
        }
    }
}