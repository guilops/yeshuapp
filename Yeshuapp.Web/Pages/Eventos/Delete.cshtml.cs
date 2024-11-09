using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Dtos;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public EventoDto Evento { get; set; }
        private readonly EventosServices _eventoServices;

        private readonly JsonSerializerOptions options;

        public DeleteModel(EventosServices eventoServices)
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

            return RedirectToPage("/Produtos/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _eventoServices.DeleteEventoAsync(Evento.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Eventos/Index");

            return RedirectToPage("/Eventos/Index");
        }
    }
}