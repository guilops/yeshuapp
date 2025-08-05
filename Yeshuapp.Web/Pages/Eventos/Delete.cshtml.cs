using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public EventoDto Evento { get; set; }
        private readonly EventosServices _eventosServices;

        private readonly JsonSerializerOptions options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteModel(EventosServices eventoServices,
                           IHttpContextAccessor httpContextAccessor)
        {
            _eventosServices = eventoServices;
            _httpContextAccessor = httpContextAccessor;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _eventosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _eventosServices.GetEventoByIdAsync(id);

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
            _eventosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _eventosServices.DeleteEventoAsync(Evento.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Eventos/Index");

            return RedirectToPage("/Eventos/Index");
        }
    }
}