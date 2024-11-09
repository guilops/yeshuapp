using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public EventoDto? Evento { get; set; }
        private readonly EventosServices _eventoServices;

        private readonly JsonSerializerOptions options;

        public EditModel(EventosServices eventoServices)
        {
            _eventoServices = eventoServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _eventoServices.GetEventoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Evento = JsonSerializer.Deserialize<EventoDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Eventos/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _eventoServices.UpdateEventoAsync(Evento);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Eventos/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar o evento.");
            return Page();
        }
    }
}