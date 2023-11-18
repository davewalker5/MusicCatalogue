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
        private readonly IBackgroundQueue<ArtistStatisticsExportWorkItem> _artistStatisticsQueue;
        private readonly IBackgroundQueue<GenreStatisticsExportWorkItem> _genreStatisticsQueue;
        private readonly IBackgroundQueue<MonthlySpendExportWorkItem> _monthlySpendQueue;

        public ExportController(
            IBackgroundQueue<CatalogueExportWorkItem> catalogueQueue,
            IBackgroundQueue<ArtistStatisticsExportWorkItem> artistStatisticsQueue,
            IBackgroundQueue<GenreStatisticsExportWorkItem> genreStatisticsQueue,
            IBackgroundQueue<MonthlySpendExportWorkItem> monthlySpendQueue
            )
        {
            _catalogueQueue = catalogueQueue;
            _artistStatisticsQueue = artistStatisticsQueue;
            _genreStatisticsQueue = genreStatisticsQueue;
            _monthlySpendQueue = monthlySpendQueue;
        }

        [HttpPost]
        [Route("catalogue")]
        public IActionResult ExportCatalogue([FromBody] CatalogueExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Catalogue Export";

            // Queue the work item
            _catalogueQueue.Enqueue(item);
            return Accepted();
        }

        [HttpPost]
        [Route("artiststatistics")]
        public IActionResult ExportArtistStatisticsReport([FromBody] ArtistStatisticsExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Artist Statistics Export";

            // Queue the work item
            _artistStatisticsQueue.Enqueue(item);
            return Accepted();
        }

        [HttpPost]
        [Route("genrestatistics")]
        public IActionResult ExportGenreStatisticsReport([FromBody] GenreStatisticsExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Genre Statistics Export";

            // Queue the work item
            _genreStatisticsQueue.Enqueue(item);
            return Accepted();
        }

        [HttpPost]
        [Route("monthlyspend")]
        public IActionResult ExportMonthySpendReport([FromBody] MonthlySpendExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Monthly Spending Export";

            // Queue the work item
            _monthlySpendQueue.Enqueue(item);
            return Accepted();
        }
    }
}
