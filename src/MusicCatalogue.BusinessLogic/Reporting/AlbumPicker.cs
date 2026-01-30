using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.BusinessLogic.Reporting
{
    [ExcludeFromCodeCoverage]
    public class AlbumPicker // : IAlbumPicker
    {
        private readonly IMusicCatalogueFactory _factory;

        public AlbumPicker(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="targetArtistId"></param>
        /// <param name="n"></param>
        /// <param name="excludeTarget"></param>
        /// <returns></returns>
        public async Task<List<Album>> GetClosestArtistsAsync(SimilarityWeights weights, int? genreId)
        {
            // If the genre is specified:
            // load all albums for the genre
            // load all artists for the albums
            return [];
        }
    }
}