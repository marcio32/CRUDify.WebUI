using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Login
{
    [Authorize]
    public class LogoutModel : PageModel
    {
      private readonly SignInManager<UserAuthentication> _signInManager;

        public LogoutModel(SignInManager<UserAuthentication> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await  _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
