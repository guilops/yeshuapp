using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class IndexModel : PageModel
    {
        private readonly PedidosServices _pedidosServices;

        public IndexModel(PedidosServices pedidosServices)
        {
            _pedidosServices = pedidosServices;
        }
        [BindProperty]
        public List<PedidoResponseDto>? Pedidos { get; set; } = new List<PedidoResponseDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _pedidosServices.SetAuthorizationHeader(token);
            var result = await _pedidosServices.GetPedidosAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var pedidos =  await result.Content.ReadFromJsonAsync<List<PedidoResponseDto>>();
            Pedidos = pedidos;

            return Page();
        }
    }
}