using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMusicLogger _logger;

        public UsersController(IUserService userService, IMusicLogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticateModel model)
        {
            _logger.LogMessage(Severity.Debug, $"Authenticating as {model.UserName}");

            string token = await _userService.AuthenticateAsync(model.UserName, model.Password);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogMessage(Severity.Error, $"Authentication as {model.UserName} failed");
                return BadRequest();
            }

            return Ok(token);
        }
    }
}
