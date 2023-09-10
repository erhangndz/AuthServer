using AuthServer.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Configuration;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
         static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                // Yapýlandýrmalarý burada yükleyin
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<CustomTokenOptions>(hostContext.Configuration.GetSection("TokenOptions"));
                services.Configure<List<Client>>(hostContext.Configuration.GetSection("Clients"));

                // Servisleri burada yapýlandýrýn
                services.AddControllers();
            });
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
      

        var app = builder.Build();
        // Add services to the container.





        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}