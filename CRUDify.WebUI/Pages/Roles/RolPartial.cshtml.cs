using Application.Interfaces;
using CRUDify.WebUI.Pages.Products.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Roles
{
    public class RolePartialModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _rolmanager;
        public RolePartialModel(RoleManager<IdentityRole> roleManager)
        {
            _rolmanager = roleManager;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Name { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = await _rolmanager.FindByIdAsync(id);
            if (role != null)
            {
                Name = role.Name;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = new IdentityRole
            {
                Name = this.Name
            };
            var result = await _rolmanager.CreateAsync(role);


            return new JsonResult(new { success = result });

        }

        public async Task<IActionResult> OnPutAsync()
        {
            var role = await _rolmanager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return BadRequest();
            }
            role.Name = this.Name;

            var result = await _rolmanager.UpdateAsync(role);
            return new JsonResult(new { success = result });
        }

        public async Task<IActionResult> OnDeleteAsync()
        {
            var role = await _rolmanager.FindByIdAsync(Id.ToString());
            if (role == null)
            {
                return BadRequest();
            }

            var result = await _rolmanager.DeleteAsync(role);
            return new JsonResult(new { success = result });
        }

    }
}
