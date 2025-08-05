using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Pedidos
{
    public class IndexModel : PageModel
    {
        private readonly PedidosServices _pedidosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(PedidosServices pedidosServices, IHttpContextAccessor httpContextAccessor)
        {
            _pedidosServices = pedidosServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public List<PedidoResponseDto>? Pedidos { get; set; } = new List<PedidoResponseDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

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