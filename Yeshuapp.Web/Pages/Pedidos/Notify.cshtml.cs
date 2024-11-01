using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;
using Yeshuapp.Web.Enums;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class NotifyModel : PageModel
    {
        [BindProperty]
        public PedidoResponseDto Pedido { get; set; }
        private readonly PedidosServices _pedidosServices;

        private readonly JsonSerializerOptions options;

        public NotifyModel(PedidosServices pedidosServices)
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
            var response = await _pedidosServices.GetPedidoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Pedido = JsonSerializer.Deserialize<PedidoResponseDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Pedidos/Index");
        }

        public async Task<IActionResult> OnPostAsync(string metodoNotificacao)
        {
            var eCanal = (ECanalNotificacao)Enum.Parse(typeof(ECanalNotificacao), metodoNotificacao, true);
            var response = await _pedidosServices.NotificarPedidoAsync(Pedido.Id, eCanal);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Pedidos/Index");

            return RedirectToPage("/Pedidos/Index");
        }
    }
}