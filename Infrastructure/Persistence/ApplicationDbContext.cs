using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Id = "1",
                UserName = "admin",
                Email = "marcioabriola@gmail.com",
                NormalizedEmail = "marcioabriola@gmail.com",
                EmailConfirmed = true
            };

            user.PasswordHash = hasher.HashPassword(user, "1234");
           
            builder.Entity<User>().HasData(user);
        }
    }
}
