namespace MusicCatalogue.Api.Entities
{
    public class MonthlySpendExportWorkItem : BackgroundWorkItem
    {
        public string FileName { get; set; } = "";
    }
}
