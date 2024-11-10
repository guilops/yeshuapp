using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public PedidoResponseDto Pedido { get; set; }
        private readonly PedidosServices _pedidosServices;

        private readonly JsonSerializerOptions options;

        public DeleteModel(PedidosServices pedidosServices)
        {
            _pedidosServices = pedidosServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _pedidosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _pedidosServices.GetPedidoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Pedido = JsonSerializer.Deserialize<PedidoResponseDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Pedidos/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _pedidosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _pedidosServices.DeletePedidoAsync(Pedido.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Pedidos/Index");

            return RedirectToPage("/Pedidos/Index");
        }
    }
}