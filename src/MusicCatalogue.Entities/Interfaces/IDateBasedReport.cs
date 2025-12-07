using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IDateBasedReport<T> where T : ReportEntityBase
    {
        Task<IEnumerable<T>> GenerateReportAsync(int year, int month, int day, int pageNumber, int pageSize);
    }
}