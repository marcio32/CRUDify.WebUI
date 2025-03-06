using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class UserAuthentication : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
