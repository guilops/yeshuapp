using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Yeshuapp.Web.Dtos;

namespace Yeshuapp.Web.Pages.Produtos
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public ProdutoDto Produto { get; set; }
        private readonly ProdutosServices _produtosServices;

        private readonly JsonSerializerOptions options;

        public DeleteModel(ProdutosServices produtosServices)
        {
            _produtosServices = produtosServices;
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

        public async Task<IActionResult> OnPostAsync()
        {
            _produtosServices.SetAuthorizationHeader(Request.Cookies["jwtToken"]);
            var response = await _produtosServices.DeleteProdutoAsync(Produto.Id);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Produtos/Index");

            return RedirectToPage("/Produtos/Index");
        }
    }
}