namespace MusicCatalogue.Api.Entities
{
    public class GenreAlbumsExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public int GenreId { get; set; }
    }
}
