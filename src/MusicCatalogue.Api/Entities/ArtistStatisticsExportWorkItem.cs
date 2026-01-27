namespace MusicCatalogue.Api.Entities
{
    public class ArtistStatisticsExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public bool WishList { get; set; }
    }
}
