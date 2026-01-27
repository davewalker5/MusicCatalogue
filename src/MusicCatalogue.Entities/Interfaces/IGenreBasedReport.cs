namespace MusicCatalogue.Entities.Interfaces
{
    public interface IGenreBasedReport<T> where T : class
    {
        Task<IEnumerable<T>> GenerateReportAsync(int genreId, int pageNumber, int pageSize);
    }
}
