using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Products
{
    public class ProductPartialModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        public ProductPartialModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Image { get; set; }
        [BindProperty]
        public int Stock { get; set; }
        [BindProperty]
        public bool Active { get; set; }

       
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var product = await _productRepository.GetByIdAsync(int.Parse(id));

                if (product != null)
                {
                    Id = product.Id;
                    Name = product.Name;
                    Price = product.Price;
                    Description = product.Description;
                    Image = product.Image;
                    Stock = product.Stock;
                    Active = product.Active;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var product = new Product
            {
                Name = this.Name,
                Price = this.Price,
                Description = this.Description,
                Image = this.Image,
                Stock = this.Stock,
                Active = Request.Form["Active"] == "on"
            };

            await _productRepository.AddAsync(product);
            return new JsonResult(new { success = true });
        }


        public async Task<IActionResult> OnPutAsync()
         {
            var product = await _productRepository.GetByIdAsync(Id);

            if(product == null)
            {
                return BadRequest();
            }

            product.Name = Name;
            if (decimal.TryParse(Request.Form["Price"], NumberStyles.Any,CultureInfo.InvariantCulture, out var price)){
                product.Price = price;
            }
           
            product.Description = Description;
            product.Image = Image;
            product.Stock = Stock;
            product.Active = Request.Form["Active"] == "on";
            await _productRepository.UpdateAsync(product);
            return new JsonResult(new { success = true });
        }
    }
}
