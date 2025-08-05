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

        public IndexModel(EventosServices eventoServices)
        {
            _eventosServices = eventoServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["jwtToken"];

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