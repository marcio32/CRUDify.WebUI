using Application.Interfaces;
using CRUDify.WebUI.Pages.Services.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Services
{
    public class ServicePartialModel : PageModel
    {
        private readonly IServiceRepository _serviceRepository;

        public ServicePartialModel(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [BindProperty]
        public  int Id{ get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public bool Activo { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = await _serviceRepository.GetByIdAsync(int.Parse(id));

                if(service != null)
                {
                    Id = service.Id;
                    Nombre = service.Nombre;
                    Activo = service.Activo;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var service = new Service
            {
                Id = this.Id,
                Nombre = this.Nombre,
                Activo = Request.Form["Activo"] == "on"
            };

            await _serviceRepository.AddAsync(service);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPutAsync()
        {
            var service = await _serviceRepository.GetByIdAsync(Id);

            if(service == null)
            {
                return BadRequest();
            }

            service.Nombre = Nombre;
            service.Activo = Request.Form["Activo"] == "on";

            await _serviceRepository.UpdateAsync(service);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnDeleteServiceAsync([FromBody] DeleteServiceRequest request)
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id);
            if(service == null)
            {
                return BadRequest();
            }
            service.Activo = false;

            await _serviceRepository.DeleteAsync(service);

            return new JsonResult(new { success = true });
        }

    }
}
