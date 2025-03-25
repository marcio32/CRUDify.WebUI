using CRUDify.WebUI.Middlewares;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace CRUDify.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"), rollingInterval: RollingInterval.Day).CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddInfrastructure(connectionString);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
            });

            var authorizationPolicies = builder.Configuration.GetSection("AuthorizationPolicies").Get<Dictionary<string, string[]>>();
            builder.Services.AddAuthorization(options =>
            {
                foreach (var policy in authorizationPolicies)
                {
                    options.AddPolicy(policy.Key, policyBuilder =>
                    {
                        policyBuilder.RequireRole(policy.Value);
                    });
                }
            });

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();

            app.Run();
        }
    }
}
