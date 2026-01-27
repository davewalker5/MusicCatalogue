using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.BusinessLogic.Config;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class SecretsController : Controller
    {
        private readonly MusicApplicationSettings _settings;
        private readonly IMusicLogger _logger;

        public SecretsController(IOptions<MusicApplicationSettings> settings, IMusicLogger logger)
        {
            _settings = settings.Value;
            _logger = logger;
            SecretResolver.ResolveAllSecrets(_settings);
        }

        /// <summary>
        /// Return a secret from the configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{name}")]
        public ActionResult<string?> GetSecret(string name)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving named secret '{name}'");

            var secret = _settings.Secrets.FirstOrDefault(x => x.Name == name); 

            if (secret == null)
            {
                _logger.LogMessage(Severity.Error, $"No matching secret found");
                return NotFound();
            }

            if (string.IsNullOrEmpty(secret.Value))
            {
                _logger.LogMessage(Severity.Warning, $"Secret is blank/empty");
                return NoContent();
            }

            return secret.Value;
        }
    }
}
