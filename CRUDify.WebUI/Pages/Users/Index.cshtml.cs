using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDify.WebUI.Pages.Users
{
    public class IndexModel : PageModel
    {
        public readonly IUserRepository _userRepository;
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IndexModel(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task OnGetAsync() => Users = await _userRepository.GetAllAsync();
    }
}
