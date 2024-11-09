using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Yeshuapp.Dtos;

namespace Yeshuapp.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FrasesServices _frasesServices;

        [BindProperty]
        public List<FraseDto> Frases { get; set; } = new List<FraseDto>();

        public IndexModel(FrasesServices frasesServices)
        {
            _frasesServices = frasesServices;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await _frasesServices.GetFrasesAsync();

            if (result.IsSuccessStatusCode)
            {
                var frases = await result.Content.ReadFromJsonAsync<List<FraseDto>>();

                Random random = new Random();
                Frases = frases.OrderBy(x => random.Next()).Take(5).Where(x=> x.Ativa).ToList();

                return Page();
            }

            return Page();
        }
    }
}
