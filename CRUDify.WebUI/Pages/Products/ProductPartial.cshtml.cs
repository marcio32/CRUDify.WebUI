using Application.Interfaces;
using CRUDify.WebUI.Pages.Products.Request;
using Domain.Entities;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Products
{
    [Authorize(Roles = "Admin, User")]
    public class ProductPartialModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<ProductHub> _hubContext;
        public ProductPartialModel(IProductRepository productRepository, IHubContext<ProductHub> hubContext)
        {
            _productRepository = productRepository;
            _hubContext = hubContext;
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
        public IFormFile Image { get; set; }
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
                Stock = this.Stock,
                Active = Request.Form["Active"] == "on"
            };

            if(Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Image.CopyToAsync(memoryStream);
                    product.Image = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            await _productRepository.AddAsync(product);
            await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", product);
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
            product.Stock = Stock;
            product.Active = Request.Form["Active"] == "on";

            if(Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Image.CopyToAsync(memoryStream);
                    product.Image = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            await _productRepository.UpdateAsync(product);
            await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", product);
            return new JsonResult(new { success = true });
        }


        public async Task<IActionResult> OnDeleteProductAsync([FromBody] DeleteProductRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                return BadRequest();
            }

            product.Active = false;

            await _productRepository.DeleteAsync(product);
            await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", product);
            return new JsonResult(new { success = true });
        }
    }
}
