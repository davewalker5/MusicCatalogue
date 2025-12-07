namespace MusicCatalogue.Api.Entities
{
    public class RetailerStatisticsExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
        public bool WishList { get; set; }
    }
}
