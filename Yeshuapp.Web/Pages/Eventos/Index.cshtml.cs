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

        private readonly EventosServices _eventoServices;

        public IndexModel(EventosServices eventoServices)
        {
            _eventoServices = eventoServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            // Lista de eventos
            var result = await _eventoServices.GetEventosAsync();

            if (!result.IsSuccessStatusCode)
                return Page();

            var eventos = await result.Content.ReadFromJsonAsync<List<EventoDto>>();
            Eventos = eventos;

            return Page();
        }
    }
}