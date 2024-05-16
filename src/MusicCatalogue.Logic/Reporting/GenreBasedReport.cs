using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.Reporting
{
    [ExcludeFromCodeCoverage]
    internal class GenreBasedReport<T> : ReportManagerBase, IGenreBasedReport<T> where T : ReportEntityBase
    {
        private const string GenreIdPlaceHoder = "$genreId";

        internal GenreBasedReport(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Generate a genre based report for reporting entity type T
        /// </summary>
        /// <param name="wishlist"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GenerateReportAsync(int genreId, int pageNumber, int pageSize)
        {
            // SQL report files are named after the keyless entity type they map to with a .sql extension
            var sqlFile = $"{typeof(T).Name}.sql";

            // Load the SQL file and perform date range place-holder replacements
            var query = ReadGenreSqlReportResource(sqlFile, genreId);

            // Run the query and return the results
            var results = await GenerateReportAsync<T>(query, pageNumber, pageSize);
            return results;
        }

        /// <summary>
        /// Read the SQL report file for a sightings-based report with a wish list flag in it
        /// </summary>
        /// <param name="reportFile"></param>
        /// <param name="genreId"></param>
        /// <returns></returns>
        private static string ReadGenreSqlReportResource(string reportFile, int genreId)
        {
            // Read and return the query, replacing the date range parameters
            var query = ReadSqlResource(reportFile, new Dictionary<string, string>
            {
                { GenreIdPlaceHoder, genreId.ToString() }
            });

            return query;
        }
    }
}
