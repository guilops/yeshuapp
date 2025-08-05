using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public PedidoResponseDto Pedido { get; set; } = new PedidoResponseDto();
        [BindProperty]
        public List<ProdutoDto>? Produtos { get; set; } = new List<ProdutoDto>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public List<SelectListItem> Irmaos { get; set; }
        private readonly PedidosServices _pedidosServices;
        private readonly ProdutosServices _produtosServices;
        private readonly IrmaosServices _irmaosServices;
        public string ErrorMessage;

        public CreateModel(PedidosServices pedidoServices, ProdutosServices produtosServices, IrmaosServices irmaosServices,
                           IHttpContextAccessor httpContextAccessor)
        {
            _pedidosServices = pedidoServices;
            _produtosServices = produtosServices;
            _irmaosServices = irmaosServices;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGet()
        {
            _pedidosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            _irmaosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            _produtosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var resultProdutos = await _produtosServices.GetProdutosAsync();
            var resultIrmaos = await _irmaosServices.GetIrmaosAsync();

            if (!resultProdutos.IsSuccessStatusCode || !resultIrmaos.IsSuccessStatusCode) 
            {
                ErrorMessage = "N�o foi poss�vel carregar as informa��es necess�rias";
                return Page();
            }

            var produtos = await resultProdutos.Content.ReadFromJsonAsync<List<ProdutoDto>>();
            Produtos = produtos;


            var irmaos = await resultIrmaos.Content.ReadFromJsonAsync<List<ClienteResponseDto>>();
            Irmaos = irmaos.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Nome
            }).ToList();

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            _pedidosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            _irmaosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            _produtosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            Pedido.Data = DateTime.Now;
            Pedido.StatusPedido = Enums.EStatusPedido.Aberto;
            Pedido.Cliente = await (await _irmaosServices.GetIrmaoByIdAsync(Pedido.CodigoCliente)).Content.ReadFromJsonAsync<ClienteRequestDto>();

            var resultProdutos = await _produtosServices.GetProdutosAsync();
            var produtos = await resultProdutos.Content.ReadFromJsonAsync<List<ProdutoDto>>();
            Produtos = produtos;

            foreach (var produtoPedido in Pedido.Produtos)
            {
                var produto = Pedido.Produtos?.FirstOrDefault(x => x.Id == produtoPedido.Id);

                produto.Produto = produtos.FirstOrDefault(x => x.Id == produtoPedido.Id);
                produto.Produto.Quantidade = produto.Produto.Quantidade - produtoPedido.Quantidade;

            }

            Pedido.Valor = Pedido.Produtos.Sum(p => p.Produto.Valor * p.Quantidade);

            var response = await _pedidosServices.CreatePedidoAsync(Pedido);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Pedidos/Index");
            }

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o pedido.");
            return Page();
        }
    }
}
