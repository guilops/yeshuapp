using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages
{
    public class HomeModel : PageModel
    {
        private readonly FrasesServices _frasesServices;
        private readonly EventosServices _eventosServices;

        [BindProperty]
        public List<FraseDto> Frases { get; set; } = new List<FraseDto>();
        [BindProperty]
        public List<EventoDto> Eventos { get; set; } = new List<EventoDto>();

        public HomeModel(FrasesServices frasesServices,EventosServices eventosServices)
        {
            _frasesServices = frasesServices;
            _eventosServices = eventosServices;
        }

        public async Task<IActionResult> OnGet()
        {
            _frasesServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            _eventosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);

            var result = await _frasesServices.GetFrasesAsync();

            var eventosResult = await _eventosServices.GetEventosAsync();

            if (result.IsSuccessStatusCode)
            {
                Frases = await result.Content.ReadFromJsonAsync<List<FraseDto>>();

                Random random = new Random();
                var eventos = await eventosResult.Content.ReadFromJsonAsync<List<EventoDto>>();

                if (eventos != null && eventos.Any())
                    Eventos = eventos.OrderBy(e => random.Next()).Take(3).ToList();
                

                return Page();
            }
            return Page();
        }
    }
}
