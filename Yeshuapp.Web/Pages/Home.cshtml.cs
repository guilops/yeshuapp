using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages
{
    public class HomeModel : PageModel
    {
        private readonly FrasesServices _frasesServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public List<FraseDto> Frases { get; set; } = new List<FraseDto>();

        public HomeModel(FrasesServices frasesServices,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _frasesServices = frasesServices;
        }

        public async Task<IActionResult> OnGet()
        {
            _frasesServices.SetAuthorizationHeader(_httpContextAccessor.HttpContext.Session.GetString("JwtToken"));
            var result = await _frasesServices.GetFrasesAsync();

            if (result.IsSuccessStatusCode)
            {
                var frases = await result.Content.ReadFromJsonAsync<List<FraseDto>>();

                Random random = new Random();
                Frases = frases.OrderBy(x => random.Next()).Take(5).Where(x => x.Ativa).ToList();

                return Page();
            }
            return Page();
        }
    }
}
