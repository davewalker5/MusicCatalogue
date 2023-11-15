using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using System.Web;

namespace MusicCatalogue.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("[controller]")]
    public class ReportsController : Controller
    {
        private const string DateTimeFormat = "yyyy-MM-dd H:mm:ss";

        private readonly IMusicCatalogueFactory _factory;

        public ReportsController(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Generate the job statistics report
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("jobs/{start}/{end}")]
        public async Task<ActionResult<List<JobStatus>>> GetJobsReportAsync(string start, string end)
        {
            // Decode the start and end date and convert them to dates
            DateTime startDate = DateTime.ParseExact(HttpUtility.UrlDecode(start), DateTimeFormat, null);
            DateTime endDate = DateTime.ParseExact(HttpUtility.UrlDecode(end), DateTimeFormat, null);

            // Get the report content
            var results = await _factory.JobStatuses
                                        .ListAsync(x => (x.Start >= startDate) && ((x.End == null) || (x.End <= endDate)),
                                                   1,
                                                   int.MaxValue)
                                        .OrderByDescending(x => x.Start)
                                        .ToListAsync();

            if (!results.Any())
            {
                return NoContent();
            }

            // Convert to a list and return the results
            return results;
        }

        /// <summary>
        /// Generate the genre statistics report
        /// </summary>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("genres/{wishlist}")]
        public async Task<ActionResult<List<GenreStatistics>>> GetGenreStatisticsReportAsync(bool wishlist)
        {
            // Get the report content
            var results = await _factory.GenreStatistics.GenerateReportAsync(wishlist, 1, int.MaxValue);

            if (!results.Any())
            {
                return NoContent();
            }

            // Convert to a list and return the results
            return results.ToList();
        }

        /// <summary>
        /// Generate the artist statistics report
        /// </summary>
        /// <param name="wishlist"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("artists/{wishlist}")]
        public async Task<ActionResult<List<ArtistStatistics>>> GetArtistStatisticsReportAsync(bool wishlist)
        {
            // Get the report content
            var results = await _factory.ArtistStatistics.GenerateReportAsync(wishlist, 1, int.MaxValue);

            if (!results.Any())
            {
                return NoContent();
            }

            // Convert to a list and return the results
            return results.ToList();
        }

        /// <summary>
        /// Generate the monthly spending report
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("spend/{wishlist}")]
        public async Task<ActionResult<List<MonthlySpend>>> GetMonthlySpendingReportAsync(bool wishlist)
        {
            // Get the report content
            var results = await _factory.MonthlySpend.GenerateReportAsync(wishlist, 1, int.MaxValue);

            if (!results.Any())
            {
                return NoContent();
            }

            // Convert to a list and return the results
            return results.ToList();
        }
    }
}
