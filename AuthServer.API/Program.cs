using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repositories;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

                var tokenOptions= hostContext.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
               services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {

                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience[0],
                        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ClockSkew=TimeSpan.Zero

                    };
        

        });


                services.Configure<List<Client>>(hostContext.Configuration.GetSection("Clients"));

                // Servisleri burada yapýlandýrýn
              
            });
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
        builder.Services.AddScoped<IUserService,UserService>();
        builder.Services.AddScoped<ITokenService,TokenService>();
        builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
        builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(GenericService<,>));
        


        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


        


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

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}