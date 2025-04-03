using CRUDify.WebUI.Pages.Login.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Login
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [BindProperty]
        public ResetPasswordInputModel Input { get; set; }
        public bool PasswordReset { get; set; }

        public void OnGet(string token, string email)
        {
            Input = new ResetPasswordInputModel
            {
                Token = token,
                Email = email
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if(user == null)
            {
                PasswordReset = false;
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
            if (result.Succeeded)
            {
                PasswordReset = true;   
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();

        }
    }
}
