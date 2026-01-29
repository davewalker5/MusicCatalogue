using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Logging;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class EquipmentTypesController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;
        private readonly IMusicLogger _logger;

        public EquipmentTypesController(IMusicCatalogueFactory factory, IMusicLogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all the equipment types in the catalogue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<EquipmentType>>> GetEquipmentTypesAsync()
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving list of equipment types");

            var equipmentTypes = await _factory.EquipmentTypes.ListAsync(x => true);

            if (equipmentTypes == null)
            {
                return NoContent();
            }

            return equipmentTypes;
        }

        /// <summary>
        /// Return equipment type details given an equipment type ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EquipmentType>> GetEquipmentTypeByIdAsync(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Retrieving equipment type with ID {id}");

            var equipmentType = await _factory.EquipmentTypes.GetAsync(x => x.Id == id);

            if (equipmentType == null)
            {
                _logger.LogMessage(Severity.Error, $"Equipment type with ID {id} not found");
                return NotFound();
            }

            _logger.LogMessage(Severity.Debug, $"Retrieved equipment type {equipmentType}");
            return equipmentType;
        }

        /// <summary>
        /// Add an equipment type from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<EquipmentType>> AddEquipmentTypeAsync([FromBody] EquipmentType template)
        {
            _logger.LogMessage(Severity.Debug, $"Adding equipment type {template}");
            var equipmentType = await _factory.EquipmentTypes.AddAsync(template.Name);
            return equipmentType;
        }

        /// <summary>
        /// Update an equipment type from a template contained in the request body
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<EquipmentType?>> UpdateEquipmentTypeAsync([FromBody] EquipmentType template)
        {
            _logger.LogMessage(Severity.Debug, $"Updating equipment type {template}");
            var equipmentType = await _factory.EquipmentTypes.UpdateAsync(template.Id, template.Name);
            return equipmentType;
        }

        /// <summary>
        /// Delete an equipment type given their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEquipmentType(int id)
        {
            _logger.LogMessage(Severity.Debug, $"Deleting equipment type with ID {id}");

            // Check the equipment type exists, first
            var equipmentType = await _factory.EquipmentTypes.GetAsync(x => x.Id == id);
            if (equipmentType == null)
            {
                _logger.LogMessage(Severity.Error, $"Equipment type with ID {id} not found");
                return NotFound();
            }

            try
            {
                // It does, so delete them
                await _factory.EquipmentTypes.DeleteAsync(id);
            }
            catch (EquipmentTypeInUseException)
            {
                // Equipment type is in use (has equipment associated with it) so this is a bad request
                _logger.LogMessage(Severity.Error, $"Equiment type with ID {id} has equipment associated with it and cannot be deleted");
                return BadRequest();
            }

            return Ok();
        }
    }
}
