namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string uri);
        void ClearHeaders();
        void AddHeader(string name, string value);
    }
}