using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Api.Services;
using MusicCatalogue.Data;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.BusinessLogic.Api;
using MusicCatalogue.BusinessLogic.Api.TheAudioDB;
using MusicCatalogue.BusinessLogic.Collection;
using MusicCatalogue.BusinessLogic.Config;
using MusicCatalogue.BusinessLogic.Factory;
using MusicCatalogue.BusinessLogic.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace MusicCatalogue.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
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
            SecretResolver.ResolveAllSecrets(settings!);

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
            var title = $"Music Catalogue API v{info.FileVersion}";

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

            // Add the catalogue exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<CatalogueExportWorkItem>, BackgroundQueue<CatalogueExportWorkItem>>();
            builder.Services.AddHostedService<CatalogueExportService>();

            // Add the equipment exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<EquipmentExportWorkItem>, BackgroundQueue<EquipmentExportWorkItem>>();
            builder.Services.AddHostedService<EquipmentExportService>();

            // Add the artist statistics exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<ArtistStatisticsExportWorkItem>, BackgroundQueue<ArtistStatisticsExportWorkItem>>();
            builder.Services.AddHostedService<ArtistStatisticsExportService>();

            // Add the genre statistics exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<GenreStatisticsExportWorkItem>, BackgroundQueue<GenreStatisticsExportWorkItem>>();
            builder.Services.AddHostedService<GenreStatisticsExportService>();

            // Add the monthly spend report exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<MonthlySpendExportWorkItem>, BackgroundQueue<MonthlySpendExportWorkItem>>();
            builder.Services.AddHostedService<MonthlySpendExportService>();

            // Add the retailer statistics report exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<RetailerStatisticsExportWorkItem>, BackgroundQueue<RetailerStatisticsExportWorkItem>>();
            builder.Services.AddHostedService<RetailerStatisticsExportService>();

            // Add the albums by genre exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<GenreAlbumsExportWorkItem>, BackgroundQueue<GenreAlbumsExportWorkItem>>();
            builder.Services.AddHostedService<GenreAlbumsExportService>();

            // Add the albums by purchase date exporter hosted service
            builder.Services.AddSingleton<IBackgroundQueue<AlbumsByPurchaseDateExportWorkItem>, BackgroundQueue<AlbumsByPurchaseDateExportWorkItem>>();
            builder.Services.AddHostedService<AlbumsByPurchaseDateExportService>();

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
                app.UseCors(x => x.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}