using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class RetailersController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public RetailersController(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Return a list of all the retailers in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Retailer>>> GetRetailersAsync()
        {
            // Get a list of all retailers in the catalogue
            List<Retailer> retailers = await _factory.Retailers.ListAsync(x => true);

            // If there are no retailers, return a no content response
            if (!retailers.Any())
            {
                return NoContent();
            }

            return retailers;
        }

        /// <summary>
        /// Return retailer details given a retailer ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Retailer>> GetRetailerByIdAsync(int id)
        {
            var retailer = await _factory.Retailers.GetAsync(x => x.Id == id);

            if (retailer == null)
            {
                return NotFound();
            }

            return retailer;
        }

        /// <summary>
        /// Add a retailer to the catalogue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Retailer>> AddRetailerAsync([FromBody] Retailer template)
        {
            var retailer = await _factory.Retailers.AddAsync(template.Name);
            return retailer;
        }

        /// <summary>
        /// Update an existing retailer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Retailer?>> UpdateRetailerAsync([FromBody] Retailer template)
        {
            var retailer = await _factory.Retailers.UpdateAsync(template.Id, template.Name);
            return retailer;
        }

        /// <summary>
        /// Delete an existing retailer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRetailerAsync(int id)
        {
            // Make sure the retailer exists
            var retailer = await _factory.Retailers.GetAsync(x => x.Id == id);

            // If the retailer doesn't exist, return a 404
            if (retailer == null)
            {
                return NotFound();
            }

            try
            {
                // Delete the retailer
                await _factory.Retailers.DeleteAsync(id);
            }
            catch (RetailerInUseException)
            {
                // Retailer is in use so this is a bad request
                return BadRequest();
            }

            return Ok();
        }
    }
}