using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {

        public readonly IProductRepository _productRepository;
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IndexModel(IProductRepository productRepository) => _productRepository = productRepository;

        public async Task OnGetAsync() => Products = await _productRepository.GetAllAsync();

        public async Task<IActionResult> OnPostDeleteProductAsync(DeleteProductRequest request)
        {
            await _productRepository.DeleteAsync(request.Id);
            return new JsonResult(new { success = true });
        }

        public class DeleteProductRequest
        {
            public int Id { get; set; }
        }
    }
}
