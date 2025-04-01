using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Services
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository _serviceRepository;
        public IEnumerable<Service> Services { get; set; }

        public IndexModel(IServiceRepository serviceRepository) => _serviceRepository = serviceRepository;

        public async Task OnGetAsync() => Services = await _serviceRepository.GetAllAsync();
    }
}
