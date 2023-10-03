using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Api.Services;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Logic.Factory;
using System.Text;

namespace MusicCatalogue.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Read the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Configure the DB context and business logic
            builder.Services.AddScoped<MusicCatalogueDbContext>();
            builder.Services.AddDbContextPool<MusicCatalogueDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("MusicCatalogueDB"));
            });
            builder.Services.AddScoped<MusicCatalogueFactory>();

            // Configure strongly typed application settings
            IConfigurationSection section = configuration.GetSection("ApplicationSettings");
            builder.Services.Configure<MusicApplicationSettings>(section);

            // Configure JWT
            var settings = section.Get<MusicApplicationSettings>();
            byte[] key = Encoding.ASCII.GetBytes(settings!.Secret);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Configure the user authentication service
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}