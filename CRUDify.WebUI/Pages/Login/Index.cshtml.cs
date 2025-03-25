using CRUDify.WebUI.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Login
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public IndexModel(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user != null && user.LockoutEnabled == true)
            {
                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
            }

            ErrorMessage = "Usuario o contraseña incorrecta";
            return Page();
        }
    }
}
