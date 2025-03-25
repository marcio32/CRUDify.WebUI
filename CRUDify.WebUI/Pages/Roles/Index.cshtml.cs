using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Roles
{
    public class IndexModel : PageModel
    {
        public readonly RoleManager<IdentityRole> _roleRepository;
        public IEnumerable<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public IndexModel(RoleManager<IdentityRole> ROleRepository) => _roleRepository = ROleRepository;

        public async Task OnGetAsync() => Roles = _roleRepository.Roles.ToList();
    }
}
