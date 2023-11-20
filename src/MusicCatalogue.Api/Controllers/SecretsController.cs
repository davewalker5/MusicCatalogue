using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicCatalogue.Entities.Config;
using MusicCatalogue.Logic.Config;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class SecretsController : Controller
    {
        private readonly MusicApplicationSettings _settings;

        public SecretsController(IOptions<MusicApplicationSettings> settings)
        {
            _settings = settings.Value;
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
            var secret = _settings.Secrets.FirstOrDefault(x => x.Name == name); 

            if (secret == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(secret.Value))
            {
                return NoContent();
            }

            return secret.Value;
        }
    }
}
