namespace MusicCatalogue.Api.Entities
{
    public class SessionExportWorkItem : BackgroundWorkItem
    {
        public int SessionId { get; set; }
        public string FileName { get; set; } = "";
    }
}
