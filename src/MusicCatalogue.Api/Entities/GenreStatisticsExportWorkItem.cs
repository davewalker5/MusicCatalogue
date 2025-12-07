namespace MusicCatalogue.Api.Entities
{
    public class GenreStatisticsExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public bool WishList { get; set; }
    }
}
