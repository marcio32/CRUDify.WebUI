using Application.Interfaces;
using CRUDify.WebUI.Models;
using CRUDify.WebUI.Pages.Login.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.Google;
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
        private readonly IEmailService _emailService;

        public IndexModel(SignInManager<User> signInManager, UserManager<User> userManager, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        [BindProperty]
        public ForgotPasswordInputModel ForgotPasswordInput { get; set; }

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

        public async Task<IActionResult> OnPostGoogleLoginAsync()
        {
            var redirectURL = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl = "" });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectURL);
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> OnPostForgotPasswordAsync()
        {
            var user = await _userManager.FindByEmailAsync(ForgotPasswordInput.Email);
            if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Page();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page("/Login/ResetPassword", pageHandler: null, values: new { token, email = ForgotPasswordInput.Email }, protocol: Request.Scheme);
            await _emailService.SendEmailAsync(ForgotPasswordInput.Email, "Restablecer Clave", $"Por favor restablezca su clave haciendo <a href='{callbackUrl}'> click aqui </a>");
            ErrorMessage = "Se ha enviado un enlace a su mail para restablecer su clave";
            return Page();
        }
    }
}
