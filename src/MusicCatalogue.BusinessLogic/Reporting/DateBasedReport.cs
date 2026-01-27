using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.BusinessLogic.Reporting
{
    [ExcludeFromCodeCoverage]
    internal class DateBasedReport<T> : ReportManagerBase, IDateBasedReport<T> where T : ReportEntityBase
    {
        private const string YearPlaceHolder = "$year";
        private const string MonthPlaceHolder = "$month";
        private const string DayPlaceHolder = "$day";

        internal DateBasedReport(MusicCatalogueDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Generate a date based report for reporting entity type T
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GenerateReportAsync(int year, int month, int day, int pageNumber, int pageSize)
        {
            // SQL report files are named after the keyless entity type they map to with a .sql extension
            var sqlFile = $"{typeof(T).Name}.sql";

            // Load the SQL file and perform date range place-holder replacements
            var query = ReadDateBasedSqlReportResource(sqlFile, year, month, day);

            // Run the query and return the results
            var results = await GenerateReportAsync<T>(query, pageNumber, pageSize);
            return results;
        }

        /// <summary>
        /// Read the SQL report file for a date-based report
        /// </summary>
        /// <param name="reportFile"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private static string ReadDateBasedSqlReportResource(string reportFile, int year, int month, int day)
        {
            // Read and return the query, replacing the date range parameters
            var query = ReadSqlResource(reportFile, new Dictionary<string, string>
            {
                { YearPlaceHolder, year.ToString() },
                { MonthPlaceHolder, month.ToString() },
                { DayPlaceHolder, year.ToString() }
            });

            return query;
        }
    }
}
