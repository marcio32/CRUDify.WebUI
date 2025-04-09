using Application.Interfaces;
using CRUDify.WebUI.Pages.Products.Request;
using CRUDify.WebUI.Pages.Roles.Request;
using Domain.Entities;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class RolePartialModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _rolmanager;
        private readonly IHubContext<RoleHub> _hubContext;
        public RolePartialModel(RoleManager<IdentityRole> roleManager, IHubContext<RoleHub> hubContext)
        {
            _rolmanager = roleManager;
            _hubContext = hubContext;
        }
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Name { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            var role = await _rolmanager.FindByIdAsync(id);
            if (role != null)
            {
                Id = role.Id;
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
            await _hubContext.Clients.All.SendAsync("ReceiveRoleUpdate", role);

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
            await _hubContext.Clients.All.SendAsync("ReceiveRoleUpdate", role);
            return new JsonResult(new { success = result });
        }

        public async Task<IActionResult> OnDeleteAsync([FromBody] DeleteRoleRequest deleteRoleRequest)
        {
            var role = await _rolmanager.FindByIdAsync(deleteRoleRequest.Id);
            if (role == null)
            {
                return BadRequest();
            }

            var result = await _rolmanager.DeleteAsync(role);
            await _hubContext.Clients.All.SendAsync("ReceiveRoleUpdate", new { deleted = true, id = role.Id });
            return new JsonResult(new { success = result });
        }

    }
}
