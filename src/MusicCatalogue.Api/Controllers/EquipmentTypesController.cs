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
    public class EquipmentTypesController : Controller
    {
        private readonly IMusicCatalogueFactory _factory;

        public EquipmentTypesController(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Return a list of all the equipment types in the catalogue matching the filter criteria in the request body
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<EquipmentType>>> GetEquipmentTypesAsync()
        {
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
            var equipmentType = await _factory.EquipmentTypes.GetAsync(x => x.Id == id);

            if (equipmentType == null)
            {
                return NotFound();
            }

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
            // Check the equipment type exists, first
            var equipmentType = await _factory.EquipmentTypes.GetAsync(x => x.Id == id);
            if (equipmentType == null)
            {
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
                return BadRequest();
            }

            return Ok();
        }
    }
}
