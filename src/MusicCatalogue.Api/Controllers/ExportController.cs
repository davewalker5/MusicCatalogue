using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Api.Entities;
using MusicCatalogue.Api.Interfaces;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class ExportController : Controller
    {
        private readonly IBackgroundQueue<CatalogueExportWorkItem> _catalogueQueue;

        public ExportController(IBackgroundQueue<CatalogueExportWorkItem> catalogueQueue)
        {
            _catalogueQueue = catalogueQueue;
        }

        [HttpPost]
        [Route("catalogue")]
        public IActionResult Export([FromBody] CatalogueExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Catalogue Export";

            // Queue the work item
            _catalogueQueue.Enqueue(item);
            return Accepted();
        }
    }
}
