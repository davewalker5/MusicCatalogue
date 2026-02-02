using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class EquipmentController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public EquipmentController(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Return a list of all items of equipment matching the filter criteria
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentAsync([FromBody] EquipmentSearchCriteria criteria)
        {
            // Ideally, this method would use the GET verb but as more filtering criteria are added that leads
            // to an increasing number of query string parameters and a very messy URL. So the filter criteria
            // are POSTed in the request body, instead, and bound into a strongly typed criteria object

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving equipment matching criteria {criteria}");

            var equipment = await _factory.Search.EquipmentSearchAsync(criteria);

            if (equipment == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"No matching equipment found");
                return NoContent();
            }

            return equipment;
        }

        /// <summary>
        /// Return equipment details given an equipment type ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Equipment>> GetEquipmentByIdAsync(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Retrieving equipment with ID {id}");

            var equipment = await _factory.Equipment.GetAsync(x => x.Id == id);

            if (equipment == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Equipment with ID {id} not found");
                return NotFound();
            }

            _factory.Logger.LogMessage(Severity.Debug, $"Retrieved equipment {equipment}");
            return equipment;
        }

        /// <summary>
        /// Add an item of equipment from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Equipment>> AddEquipmentAsync([FromBody] Equipment template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Adding equiment {template}");
            var equipment = await _factory.Equipment.AddAsync(
                template.EquipmentTypeId,
                template.ManufacturerId,
                template.Description,
                template.Model,
                template.SerialNumber,
                template.IsWishListItem,
                template.Purchased,
                template.Price,
                template.RetailerId);
            return equipment;
        }

        /// <summary>
        /// Update an item of equipment from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<Equipment?>> UpdateEquipmentAsync([FromBody] Equipment template)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Updating equiment {template}");
            var equipment = await _factory.Equipment.UpdateAsync(
                template.Id,
                template.EquipmentTypeId,
                template.ManufacturerId,
                template.Description,
                template.Model,
                template.SerialNumber,
                template.IsWishListItem,
                template.Purchased,
                template.Price,
                template.RetailerId);
            return equipment;
        }

        /// <summary>
        /// Delete an item of equipment given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            _factory.Logger.LogMessage(Severity.Debug, $"Deleting equipment with ID {id}");

            // Check the equipment exists, first
            var equipment = await _factory.Equipment.GetAsync(x => x.Id == id);
            if (equipment == null)
            {
                _factory.Logger.LogMessage(Severity.Error, $"Equipment with ID {id} not found");
                return NotFound();
            }

            // It does, so delete it
            await _factory.Equipment.DeleteAsync(id);

            return Ok();
        }
    }
}
