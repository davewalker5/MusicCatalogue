using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Api.Services;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Logic.Api;
using MusicCatalogue.Logic.Api.TheAudioDB;
using MusicCatalogue.Logic.Collection;
using MusicCatalogue.Logic.Config;
using MusicCatalogue.Logic.Factory;
using MusicCatalogue.Logic.Logging;
using System.Diagnostics;
using System.Reflection;
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

            // Configure strongly typed application settings
            IConfigurationSection section = configuration.GetSection("ApplicationSettings");
            builder.Services.Configure<MusicApplicationSettings>(section);
            var settings = section.Get<MusicApplicationSettings>();
            ApiKeyResolver.ResolveAllApiKeys(settings!);

            // Configure the DB context
            builder.Services.AddScoped<MusicCatalogueDbContext>();
            builder.Services.AddDbContextPool<MusicCatalogueDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("MusicCatalogueDB"));
            });

            // Get the API key and the URLs for the album and track lookup endpoints
            var apiKey = settings!.ApiServiceKeys.Find(x => x.Service == ApiServiceType.TheAudioDB)!.Key;
            var albumsEndpoint = settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Albums)!.Url;
            var tracksEndpoint = settings.ApiEndpoints.Find(x => x.EndpointType == ApiEndpointType.Tracks)!.Url;

            // Convert the URL into a URI instance that will expose the host name - this is needed
            // to set up the client headers
            var uri = new Uri(albumsEndpoint);

            // Get the version number and application title
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var title = $"Music Catalogue Lookup Tool v{info.FileVersion}";

            // Create the file logger and log the startup messages
            var logger = new FileLogger();
            logger.Initialise(settings!.LogFile, settings.MinimumLogLevel);
            logger.LogMessage(Severity.Info, new string('=', 80));
            logger.LogMessage(Severity.Info, title);

            // Register the logger with the DI framework
            builder.Services.AddSingleton<IMusicLogger>(x => logger);

            // Configure the HTTP client used by the external APIs
            builder.Services.AddSingleton<IMusicHttpClient>(x =>
            {
                var client = MusicHttpClient.Instance;
                client.AddHeader("X-RapidAPI-Key", apiKey);
                client.AddHeader("X-RapidAPI-Host", uri.Host);
                return client;
            });

            // Configure the external APIs
            builder.Services.AddScoped<IAlbumsApi>(x => new TheAudioDBAlbumsApi(
                logger: x.GetRequiredService<IMusicLogger>(),
                client: x.GetRequiredService<IMusicHttpClient>(),
                url: albumsEndpoint));

            builder.Services.AddScoped<ITracksApi>(x => new TheAudioDBTracksApi(
                logger: x.GetRequiredService<IMusicLogger>(),
                client: x.GetRequiredService<IMusicHttpClient>(),
                url: tracksEndpoint));

            // Configure the business logic
            builder.Services.AddScoped<IMusicCatalogueFactory, MusicCatalogueFactory>();
            builder.Services.AddScoped<IAlbumLookupManager, AlbumLookupManager>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Configure JWT
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Allow the development React application to access the service
            if (settings.Environment == MusicCatalogueEnvironment.Development)
            {
                app.UseCors(x => x.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}