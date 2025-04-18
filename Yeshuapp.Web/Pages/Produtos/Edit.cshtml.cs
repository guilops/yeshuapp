using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Produtos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ProdutoDto? Produto { get; set; }
        private readonly ProdutosServices _produtosServices;

        private readonly JsonSerializerOptions options;

        public EditModel(ProdutosServices irmaosServices)
        {
            _produtosServices = irmaosServices;
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _produtosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _produtosServices.GetProdutoByIdAsync(id);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Produto = JsonSerializer.Deserialize<ProdutoDto>(json, options);
                return Page();
            }

            return RedirectToPage("/Produtos/Index");
        }

        public async Task<IActionResult> OnPostAsync(IFormFile imagemFile)
        {
            _produtosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            if (imagemFile != null && imagemFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagemFile.CopyToAsync(memoryStream);
                    Produto.Imagem = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            var response = await _produtosServices.UpdateProdutoAsync(Produto);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Produtos/Index");

            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao editar o cliente.");
            return Page();
        }
    }
}