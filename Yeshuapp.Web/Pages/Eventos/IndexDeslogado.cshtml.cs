using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class IndexDeslogadoModel : PageModel
    {
        [BindProperty]
        public List<EventoDto>? Eventos { get; set; }

        private readonly EventosServices _eventosServices;

        public IndexDeslogadoModel(EventosServices eventoServices)
        {
            _eventosServices = eventoServices;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var result = await _eventosServices.GetEventosAsync(true);

            if (!result.IsSuccessStatusCode)
                return Page();

            var eventos = await result.Content.ReadFromJsonAsync<List<EventoDto>>();
            Eventos = eventos;

            return Page();
        }
    }
}