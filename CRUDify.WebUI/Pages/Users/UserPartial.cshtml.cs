using Application.Interfaces;
using CRUDify.WebUI.Pages.Products.Request;
using Domain.Entities;
using Infrastructure.Hubs;
using Infrastructure.Repositorys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace CRUDify.WebUI.Pages.Users
{
    public class UserPartialModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHubContext<UserHub> _hubContext;
        public UserPartialModel(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IHubContext<UserHub> hubContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hubContext = hubContext;
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

            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                Id = user.Id;
                Email = user.Email;
                Roles = (await _userManager.GetRolesAsync(user)).ToList();
            }
            AllRoles = _roleManager.Roles.Select(x => x.Name).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = new User
            {

                UserName = this.Email,
                Email = this.Email,
                NormalizedEmail = this.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, this.Password);

            if (!result.Succeeded)
            {

                return new JsonResult(new { success = false });
            }

            if(Roles != null && Roles.Any())
            {
                var validRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                var rolesToAssign = Roles.Intersect(validRoles).ToList();

                if (rolesToAssign.Any())
                {
                    await _userManager.AddToRolesAsync(user, rolesToAssign);
                }
            }

            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPutAsync()
        {
            var user = await _userManager.FindByIdAsync(Id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenEmail = await _userManager.GenerateChangeEmailTokenAsync(user, this.Email);
            var resultPassword = await _userManager.ResetPasswordAsync(user, token, this.Password);
            var resultEmail = await _userManager.ChangeEmailAsync(user, Email, tokenEmail);

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = Roles.Except(userRoles).ToList();
            var rolesToRemove = userRoles.Except(Roles).ToList();


            if (rolesToRemove.Any())
            {
                var resultRemoveRoles = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                var resultAddRoles = await _userManager.AddToRolesAsync(user, rolesToAdd);
            }

            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", user);
            return new JsonResult(new { success = resultPassword.Succeeded });
        }


        public async Task<IActionResult> OnDeleteUserAsync([FromBody] DeleteUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if(user == null)
            {
                return new JsonResult(new { success = false });
            }
            var result = await _userManager.SetLockoutEnabledAsync(user, false);

            await _hubContext.Clients.All.SendAsync("ReceiveUserUpdate", new { lockoutEnabled = false, id = user.Id, email = user.Email });

            return new JsonResult(new { success = true });
        }

    }
}
