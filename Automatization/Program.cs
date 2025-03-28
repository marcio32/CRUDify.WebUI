using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositorys;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Automatization
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var productRepository = host.Services.GetRequiredService<IProductRepository>();
            var emailService = host.Services.GetRequiredService<IEmailService>();

            var products = await productRepository.GetAllAsync();

            foreach (var product in products)
            {
                if (product.Stock < 10)
                {
                    await emailService.SendEmailAsync("marcioabriola@gmail.com", "Stock Bajo", $"El stock del producto {product.Name} es menor a 10");
                }
            }
        }


        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("C:\\Users\\Marci\\OneDrive\\Escritorio\\Proyecto\\CRUDify.WebUI\\CRUDify.WebUI\\appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<IProductRepository, ProductRepository>();
                    services.Configure<EmailSettings>(context.Configuration.GetSection("EmailSettings"));
                    services.AddScoped<IEmailService, EmailService>();
                });
    }
}
