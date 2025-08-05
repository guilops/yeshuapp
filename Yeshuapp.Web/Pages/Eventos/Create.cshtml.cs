using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages.Eventos
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public EventoDto Evento { get; set; } = new EventoDto();
        private readonly EventosServices _eventosServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(EventosServices eventoServices,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventosServices = eventoServices;
        }

        public void OnGetAsync() {}

        public async Task<IActionResult> OnPostAsync()
        {
            _eventosServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var response = await _eventosServices.CreateEventoAsync(Evento);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Eventos/Index");
            }

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o evento.");
            return Page();
        }
    }
}
