using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.BusinessLogic.Reporting
{
    [ExcludeFromCodeCoverage]
    public class ArtistSimilarityCalculator : IArtistSimilarityCalculator
    {
        private readonly IMusicCatalogueFactory _factory;

        public ArtistSimilarityCalculator(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Calculate the distance between all artists in a collection and the target artist, returning
        /// the top N closest artists
        /// </summary>
        /// <param name="weights"></param>
        /// <param name="targetArtistId"></param>
        /// <param name="n"></param>
        /// <param name="excludeTarget"></param>
        /// <returns></returns>
        public async Task<List<ClosestArtist>> GetClosestArtistsAsync(
            SimilarityWeights weights,
            int targetArtistId,
            int n,
            bool excludeTarget = true)
        {
            var artists = await _factory.Artists.ListAsync(x => true, false);
            var closest = GetClosestArtists(artists, weights, targetArtistId, n, excludeTarget);
            return closest;
        }

        /// <summary>
        /// Calculate the distance between all artists in a collection and the target artist, returning
        /// the top N closest artists
        /// </summary>
        /// <param name="artists"></param>
        /// <param name="weights"></param>
        /// <param name="targetArtistId"></param>
        /// <param name="n"></param>
        /// <param name="excludeTarget"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<ClosestArtist> GetClosestArtists(
            IEnumerable<Artist> artists,
            SimilarityWeights weights,
            int targetArtistId,
            int n,
            bool excludeTarget = true)
        {
            // Check we have some artists and the number of artists to return is valid
            if ((artists is null) || n <= 0)
            {
                return [];
            }

            // Materialise the artist list
            var list = artists as IList<Artist> ?? [.. artists];

            // Identify the target artist
            var target = list.FirstOrDefault(a => a.Id == targetArtistId);
            if (target == null)
            {
                var message = $"Target artist with ID {targetArtistId} not found";
                throw new InvalidOperationException(message);
            }

            // Compute raw distances between artists
            var distances = list
                .Where(a => !excludeTarget || a.Id != targetArtistId)
                .Select(a => new
                {
                    Artist = a,
                    Distance = WeightedEuclideanDistance(target, a, weights)
                })
                .ToList();

            // Find the maximum distance
            var maxDistance = distances.Max(x => x.Distance);

            // Unlikely edge case, but handle the case where all artists are identical
            if (maxDistance == 0)
            {
                return [.. distances
                    .Take(n)
                    .Select(x => new ClosestArtist
                    {
                        Artist = x.Artist,
                        Distance = 0,
                        Similarity = 100
                    })];
            }

            // Normalise the raw distances to a % similarity
            return [.. distances
                .Select(x => new ClosestArtist
                {
                    Artist = x.Artist,
                    Distance = x.Distance,
                    Similarity = Math.Round((1.0 - (x.Distance / maxDistance)) * 100, 2)
                })
                .OrderByDescending(x => x.Similarity)
                .ThenBy(x => x.Artist.Name)
                .Take(n)];
        }

        /// <summary>
        /// Calculate the distance between two artists
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static double WeightedEuclideanDistance(Artist a, Artist b, SimilarityWeights w)
        {
            // Each artist is treated as a point in 3D space with energy along the X axis, intimacy along
            // the Y axis and warmth along the Z axis. Calculate how different the two artists are along
            // each axis
            var dE = (a.Energy - b.Energy) * w.Energy;
            var dI = (a.Intimacy - b.Intimacy) * w.Intimacy;
            var dW = (a.Warmth - b.Warmth) * w.Warmth;

            // Then square and sum the squares to give the squared Euclidian distance. Using the square
            // removes negative values and amplifies larger differences and we can then take the square root
            // to give the Euclidian distance
            return Math.Sqrt(dE * dE + dI * dI + dW * dW);
        }
    }
}