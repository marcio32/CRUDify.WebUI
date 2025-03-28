using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {

        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IndexModel(IProductRepository productRepository, IEmailService emailService)
        {

            _productRepository = productRepository;
            _emailService = emailService;
        }
        public async Task OnGetAsync()
        {
            Products = await _productRepository.GetAllAsync();

            foreach (var product in Products)
            {
                if (product.Stock < 10)
                {
                   _emailService.SendEmailAsync("marcioabriola@gmail.com", "Stock Bajo", $"El stock del producto {product.Name} es menor a 10");
                }
            }

        }


    }
}
