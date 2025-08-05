using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<EventoDto>? Eventos { get; set; }

        private readonly EventosServices _eventosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(EventosServices eventoServices,
                         IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventosServices = eventoServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            _eventosServices.SetAuthorizationHeader(token);
            var result = await _eventosServices.GetEventosAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var eventos = await result.Content.ReadFromJsonAsync<List<EventoDto>>();
            Eventos = eventos;

            return Page();
        }
    }
}