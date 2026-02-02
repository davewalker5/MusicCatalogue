using MusicCatalogue.Entities.Database;

namespace MusicCatalogue.Api.Extensions
{
    public static class EnsembleTypeExtensions
    {
        /// <summary>
        /// Convert an ensemble type to a descriptive string
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public static string ToName(this EnsembleType type)
        {
            return type switch
            {
                EnsembleType.Unknown => "",
                EnsembleType.Band => "Band",
                EnsembleType.BigBand => "Big Band",
                EnsembleType.Choir => "Choir",
                EnsembleType.Orchestra => "Orchestra",
                EnsembleType.Quartet => "Quartet",
                EnsembleType.SmallCombo => "Small Combo",
                EnsembleType.Solo => "Solo",
                EnsembleType.Studio => "Studio",
                EnsembleType.Trio => "Trio",
                _ => "",
            };
        }
    }
}