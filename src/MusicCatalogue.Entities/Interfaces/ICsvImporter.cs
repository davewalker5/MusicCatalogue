namespace MusicCatalogue.Entities.Interfaces
{
    public interface ICsvImporter
    {
        Task Import(string file);
    }
}