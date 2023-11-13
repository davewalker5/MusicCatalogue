namespace MusicCatalogue.Entities.Interfaces
{
    public interface IWishListBasedReport<T> where T : class
    {
        Task<IEnumerable<T>> GenerateReportAsync(bool wishlist, int pageNumber, int pageSize);
    }
}