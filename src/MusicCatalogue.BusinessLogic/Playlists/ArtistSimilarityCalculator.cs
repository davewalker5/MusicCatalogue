using System.Diagnostics.CodeAnalysis;
using MusicCatalogue.BusinessLogic.Extensions;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.BusinessLogic.Playlists
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
            // Identify the target artist
            var target = artists.FirstOrDefault(a => a.Id == targetArtistId);
            if (target == null)
            {
                var message = $"Target artist with ID {targetArtistId} not found";
                throw new InvalidOperationException(message);
            }

            // Find and return the closest matches
            var closest = GetClosestArtists(artists, weights, target, n, excludeTarget);
            return closest;
        }

        /// <summary>
        /// Calculate the distance between all artists in a collection and the target artist, returning
        /// the top N closest artists
        /// </summary>
        /// <param name="artists"></param>
        /// <param name="weights"></param>
        /// <param name="target"></param>
        /// <param name="n"></param>
        /// <param name="excludeTarget"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<ClosestArtist> GetClosestArtists(
            IEnumerable<Artist> artists,
            SimilarityWeights weights,
            Artist target,
            int n,
            bool excludeTarget = true)
        {
            // Make sure the search criteria are valid
            if ((artists?.Count() == 0) || (n <= 0) || !weights.HaveWeights())
            {
                return [];
            }

            // Materialise the artist list
            var list = artists as IList<Artist> ?? [.. artists!];

            // Get the target artist moods
            var targetMoodIds = GetMoodIds(target);

            // Compute raw distances between artists
            var computed = list
                .Where(a => !excludeTarget || a.Id != target.Id)
                .Select(a =>
                {
                    var numeric = WeightedEuclideanDistance(target, a, weights);
                    var moodIds = GetMoodIds(a);
                    var (moodDist, shared) = JaccardDistanceAndSharedCount(targetMoodIds, moodIds);
                    var combined = numeric + (weights.MoodWeight * moodDist);
                    return new
                    {
                        Artist = a,
                        NumericDistance = numeric,
                        MoodDistance = moodDist,
                        SharedMoods = shared,
                        Distance = combined
                    };
                })
                .ToList();

            // Find the maximum distance
            var maxDistance = computed.Max(x => x.Distance);

            return computed
                .Select(x => new ClosestArtist
                {
                    Artist = x.Artist,
                    NumericDistance = x.NumericDistance,
                    MoodDistance = x.MoodDistance,
                    SharedMoods = x.SharedMoods,

                    // Expose a combined Distance + Similarity for the caller, but they are NOT used for ordering. The
                    // similarity is populated in the Let clause, below
                    Distance = x.NumericDistance + (weights.MoodWeight * x.MoodDistance),
                    Similarity = 0
                })
                .OrderBy(x => x.NumericDistance)   // PRIMARY SORT: style profile
                .ThenBy(x => x.MoodDistance)       // SECONDARY SORT: mood overlap
                .ThenBy(x => x.Artist.Name)
                .Take(n)
                .Select((x, index) => x)
                .ToList()
                .Let(list =>
                {
                    var maxDistance = list.Max(x => x.Distance);

                    foreach (var item in list)
                    {
                        item.Similarity = maxDistance == 0
                            ? 100
                            : Math.Round((1.0 - (item.Distance / maxDistance)) * 100, 2);
                    }

                    return list;
                });
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
            var dE = (a.Energy - b.Energy) * w.EnergyWeight;
            var dI = (a.Intimacy - b.Intimacy) * w.IntimacyWeight;
            var dW = (a.Warmth - b.Warmth) * w.WarmthWeight;

            // Then square and sum the squares to give the squared Euclidian distance. Using the square
            // removes negative values and amplifies larger differences and we can then take the square root
            // to give the Euclidian distance
            return Math.Sqrt(dE * dE + dI * dI + dW * dW);
        }

        /// <summary>
        /// Jaccard similarity answers the question "How similar are two sets (of moods)?" by
        /// calculating (number of shared items)/(number of unique items across both sets). The
        /// value is 0 = identical sets, 1 = no overlap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static (double Distance, int Shared) JaccardDistanceAndSharedCount(HashSet<int> a, HashSet<int> b)
        {
            // If both artists have no moods, treate them as identical
            if (a.Count == 0 && b.Count == 0) return (0.0, 0);

            // If one artist has moods and the other doesn't, treat them as maximum mismatch
            if (a.Count == 0 || b.Count == 0) return (1.0, 0);

            // Determine which is the smallest vs largest set
            var smallest = a.Count <= b.Count ? a : b;
            var largest = a.Count <= b.Count ? b : a;

            // Iterate over the smallest set
            int intersection = 0;
            foreach (var x in smallest)
            {
                if (largest.Contains(x))
                {
                    intersection++;
                }
            }

            // Compute the union size and convert to similarity
            var union = a.Count + b.Count - intersection;
            var similarity = intersection / (double)union;
            return (1.0 - similarity, intersection);
        }

        /// <summary>
        /// Get the mood IDs for a specified artist
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        private static HashSet<int> GetMoodIds(Artist artist)
            => [.. artist.Moods.Select(am => am.MoodId)];
    }
}