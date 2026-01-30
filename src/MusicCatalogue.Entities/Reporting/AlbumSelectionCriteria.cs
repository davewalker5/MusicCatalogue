using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Entities.Reporting
{
    [ExcludeFromCodeCoverage]
    public class AlbumSelectionCriteria : SimilarityWeights
    {
        public int? GenreId { get; set; }
        public int TargetEnergy { get; set; }
        public int TargetIntimacy { get; set; }
        public int TargetWarmth { get; set; }
        public int NumberOfAlbums { get; set; }
        public int NumberPerArtist { get; set; }
        public double PickerThreshold { get; set; }
    }
}