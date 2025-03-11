using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Products
{
    public class EditProductPartialModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        public EditProductPartialModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public decimal Price { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var product = await _productRepository.GetByIdAsync(int.Parse(id));

            if(product == null)
            {
                return NotFound();
            }

            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
         {
            var product = await _productRepository.GetByIdAsync(Id);

            if(product == null)
            {
                return BadRequest();
            }

            product.Name = Name;
            product.Price = Price;
            await _productRepository.UpdateAsync(product);
            return new JsonResult(new { success = true });
        }
    }
}
