using Application.Interfaces;
using CRUDify.WebUI.Pages.Products.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Users
{
    public class UsuariosPartialModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsuariosPartialModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string Id { get; set; } 
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public List<string> Roles { get; set; }
         [BindProperty]
        public List<string> AllRoles { get; set; }
 
   


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user != null)
                {
                    Id = user.Id;
                    Email = user.Email;
                    Roles = (await _userManager.GetRolesAsync(user)).ToList();
                    AllRoles = _roleManager.Roles.Select(x => x.Name).ToList();
                }
            }

            return Page();
        }

       
    }
}
