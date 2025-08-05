using System.IdentityModel.Tokens.Jwt;
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

        public string Saudacao { get; set; } = string.Empty;

        public HomeModel(FrasesServices frasesServices,EventosServices eventosServices)
        {
            _frasesServices = frasesServices;
            _eventosServices = eventosServices;
        }

        public async Task<IActionResult> OnGet()
        {
            var jwtToken = Request.Cookies["jwtToken"] ?? throw new UnauthorizedAccessException("Token JWT nao encontrado.");
            
            _frasesServices.SetAuthorizationHeader(jwtToken);
            _eventosServices.SetAuthorizationHeader(jwtToken);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var username = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value;

            var fusoHorario = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var horaBrasilia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, fusoHorario);
            var hora = horaBrasilia.Hour;

            if (hora >= 5 && hora < 12)
                Saudacao = "Bom dia";
            else if (hora >= 12 && hora < 18)
                Saudacao = "Boa tarde";
            else
                Saudacao = "Boa noite";

            if (!string.IsNullOrEmpty(username))
                Saudacao += $", {username}";

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
