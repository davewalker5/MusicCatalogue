using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.BusinessLogic.Reporting
{
    [ExcludeFromCodeCoverage]
    public class AlbumPicker : IAlbumPicker
    {
        private readonly IMusicCatalogueFactory _factory;

        public AlbumPicker(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Pick a set of albums that match the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<List<PickedAlbum>> PickAsync(AlbumSelectionCriteria criteria)
        {
            // Make sure the criteria are valid
            if (!criteria.HaveWeights() ||
                (criteria.NumberOfAlbums < 1) ||
                (criteria.NumberPerArtist < 1) ||
                (criteria.PickerThreshold < 0) ||
                (criteria.PickerThreshold > 1))
            {
                return [];
            }

            // Load the candidate albums and artists
            var albums = await LoadAlbumsAsync(criteria.GenreId);
            var artists = await LoadArtistsAsync(albums);

            // Synthesise an artist to match against from the supplied style properties
            var artist = new Artist
            {
                Energy = criteria.TargetEnergy,
                Intimacy = criteria.TargetIntimacy,
                Warmth = criteria.TargetWarmth
            };

            // If a mood is specified, associate it with the synthesised artist. Otherwise, set
            // the mood weighting to 0
            if (criteria.MoodId != null)
            {
                artist.Moods = [
                    new ArtistMood
                    {
                        MoodId = criteria.MoodId.Value
                    }
                ];
            }
            else
            {
                criteria.MoodWeight = 0;
            }

            // Amend the incoming matching weights : If a target is 0, the weight attached to that axis is
            // set to 0
            criteria.EnergyWeight = AmendWeight(criteria.TargetEnergy, criteria.EnergyWeight);
            criteria.IntimacyWeight = AmendWeight(criteria.TargetIntimacy, criteria.IntimacyWeight);
            criteria.WarmthWeight = AmendWeight(criteria.TargetWarmth, criteria.WarmthWeight);

            // Get the closest artists, take only those with a similarity of the threshold or more and randomise them
            var closest = _factory.ArtistSimilarityCalculator.GetClosestArtists(artists, criteria, artist, artists.Count, true);
            var randomised = closest.Where(x => x.Similarity >= (100 * criteria.PickerThreshold)).OrderBy(_ => Guid.NewGuid());

            var pickedAlbums = new List<PickedAlbum>();

            // Now iterate over them and add albums from each artist, from closest match down, until
            // the required number of albums has been added or we've run out of albums
            foreach (var match in randomised)
            {
                // Calculate the maximum number needed from this artist
                var remaining = criteria.NumberOfAlbums - pickedAlbums.Count;
                var limit = Math.Min(remaining, criteria.NumberPerArtist);
                if (limit <= 0)
                {
                    break;
                }

                // Select up to that number of albums
                var pickedAlbumsToAdd = SelectAlbumsForArtist(match, albums, limit);
                pickedAlbums.AddRange(pickedAlbumsToAdd);
            }

            return pickedAlbums;
        }

        /// <summary>
        /// Amend a weight according to the required target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        private static double AmendWeight(int target, double weight)
            => target > 0 ? weight : 0;

        /// <summary>
        /// Randomise the order of albums for an artist then pick the first "N"
        /// </summary>
        /// <param name="albums"></param>
        /// <param name="artistId"></param>
        /// <param name="numberToPick"></param>
        /// <returns></returns>
        private static IEnumerable<PickedAlbum> SelectAlbumsForArtist(ClosestArtist artist, List<Album> albums, int numberToPick)
            => albums
                .Where(x => x.ArtistId == artist.Artist.Id)
                .Select(x => new PickedAlbum
                {
                    Album = x,
                    Distance = artist.Distance,
                    Similarity = artist.Similarity,
                    NumericDistance = artist.NumericDistance,
                    MoodDistance = artist.MoodDistance,
                    SharedMoods = artist.SharedMoods
                })
                .OrderBy(_ => Guid.NewGuid())
                .Take(numberToPick);

        /// <summary>
        /// Load the candidate albums. This will load every album but for a personal collection that
        /// shouldn't cause issues
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        private async Task<List<Album>> LoadAlbumsAsync(int? genreId)
            => genreId != null ? 
                await _factory.Albums.ListAsync(x => x.GenreId == genreId) :
                await _factory.Albums.ListAsync(x => true);

        /// <summary>
        /// Load the artists for the specified albums
        /// </summary>
        /// <param name="genreId"></param>
        /// <returns></returns>
        private async Task<List<Artist>> LoadArtistsAsync(List<Album> albums)
        {
            var artistIds = albums.Select(x => x.ArtistId).Distinct().ToList();
            var artists = await _factory.Artists.ListAsync(x => artistIds.Contains(x.Id), false);
            return artists;
        }
    }
}