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
        private readonly IBackgroundQueue<EquipmentExportWorkItem> _equipmentQueue;
        private readonly IBackgroundQueue<ArtistStatisticsExportWorkItem> _artistStatisticsQueue;
        private readonly IBackgroundQueue<GenreStatisticsExportWorkItem> _genreStatisticsQueue;
        private readonly IBackgroundQueue<MonthlySpendExportWorkItem> _monthlySpendQueue;
        private readonly IBackgroundQueue<RetailerStatisticsExportWorkItem> _retailerStatisticsQueue;
        private readonly IBackgroundQueue<GenreAlbumsExportWorkItem> _genreAlbumsQueue;
        private readonly IBackgroundQueue<AlbumsByPurchaseDateExportWorkItem> _albumsByPurchaseDateQueue;

        public ExportController(
            IBackgroundQueue<CatalogueExportWorkItem> catalogueQueue,
            IBackgroundQueue<EquipmentExportWorkItem> equipmentQueue,
            IBackgroundQueue<ArtistStatisticsExportWorkItem> artistStatisticsQueue,
            IBackgroundQueue<GenreStatisticsExportWorkItem> genreStatisticsQueue,
            IBackgroundQueue<MonthlySpendExportWorkItem> monthlySpendQueue,
            IBackgroundQueue<RetailerStatisticsExportWorkItem> retailerStatisticsQueue,
            IBackgroundQueue<GenreAlbumsExportWorkItem> genreAlbumsQueue,
            IBackgroundQueue<AlbumsByPurchaseDateExportWorkItem> albumsByPurchaseDateQueue)
        {
            _catalogueQueue = catalogueQueue;
            _equipmentQueue = equipmentQueue;
            _artistStatisticsQueue = artistStatisticsQueue;
            _genreStatisticsQueue = genreStatisticsQueue;
            _monthlySpendQueue = monthlySpendQueue;
            _retailerStatisticsQueue = retailerStatisticsQueue;
            _genreAlbumsQueue = genreAlbumsQueue;
            _albumsByPurchaseDateQueue = albumsByPurchaseDateQueue;
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
        [Route("equipment")]
        public IActionResult ExportEquipment([FromBody] EquipmentExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Equipment Export";

            // Queue the work item
            _equipmentQueue.Enqueue(item);
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

        [HttpPost]
        [Route("retailerstatistics")]
        public IActionResult ExportRetailerStatisticsReport([FromBody] RetailerStatisticsExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Retailer Statistics Export";

            // Queue the work item
            _retailerStatisticsQueue.Enqueue(item);
            return Accepted();
        }

        [HttpPost]
        [Route("genrealbums")]
        public IActionResult ExportGenreAlbumsReport([FromBody] GenreAlbumsExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Albums by Genre Export";

            // Queue the work item
            _genreAlbumsQueue.Enqueue(item);
            return Accepted();
        }

        [HttpPost]
        [Route("albumsByPurchaseDate")]
        public IActionResult ExportAlbumsByPurchaseDateReport([FromBody] AlbumsByPurchaseDateExportWorkItem item)
        {
            // Set the job name used in the job status record
            item.JobName = "Albums by Purchase Date Export";

            // Queue the work item
            _albumsByPurchaseDateQueue.Enqueue(item);
            return Accepted();
        }
    }
}
