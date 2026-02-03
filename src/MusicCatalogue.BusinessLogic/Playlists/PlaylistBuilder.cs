using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Extensions;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Playlists;

namespace MusicCatalogue.BusinessLogic.Playlists
{
    public sealed class PlaylistBuilder : IPlaylistBuilder
    {
        private const int MaximumStyleValue = 5;

        // Style targets by time of day
        private static readonly Dictionary<TimeOfDay, (double E, double I, double W)> _styleTargets = new()
        {
            { TimeOfDay.Morning,   (3.0, 2.0, 3.0) },
            { TimeOfDay.Afternoon, (3.5, 2.0, 3.0) },
            { TimeOfDay.Evening,   (2.5, 3.5, 3.5) },
            { TimeOfDay.Late,      (1.8, 4.2, 3.8) },
        };

        private readonly IMusicCatalogueFactory _factory;

        public PlaylistBuilder(IMusicCatalogueFactory factory)
            => _factory = factory;

        /// <summary>
        /// Build a playlist using all available artists in the catalogue
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public async Task<List<PlaylistArtist>> BuildPlaylist(PlaylistType mode, TimeOfDay timeOfDay, int n)
        {
            // Load the albums that are *not* on the wish list, extract the artist IDs and load the artists
            var albums = await _factory.Albums.ListAsync(x => !(x.IsWishListItem ?? false));
            var artistIds = albums.Select(x => x.ArtistId).ToList();
            var artists = await _factory.Artists.ListAsync(x => artistIds.Contains(x.Id), false);
            return BuildPlaylist(artists, mode, timeOfDay, n);
        }

        /// <summary>
        /// Build a playlist using the specified set of artists
        /// </summary>
        public List<PlaylistArtist> BuildPlaylist(IEnumerable<Artist> artists, PlaylistType mode, TimeOfDay timeOfDay, int n)
        {
            var parameters = PlaylistParameterResolver.Resolve(mode, timeOfDay, n);
            return BuildPlaylist(artists, parameters);
        }

        /// <summary>
        /// Pick one random album for each artist in the specified playlist
        /// </summary>
        /// <param name="artists"></param>
        /// <returns></returns>
        public async Task<List<Album>> PickPlaylistAlbums(IEnumerable<PlaylistArtist> artists)
        {
            List<Album> pickedAlbums = [];

            foreach (var playlistArtist in artists)
            {
                var artist = await _factory.Artists.GetAsync(x => x.Id == playlistArtist.ArtistId, true);
                if (artist.Albums.Count > 0)
                {
                    var album = artist.Albums.Where(x => !(x.IsWishListItem ?? false)).OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
                    album!.Artist = artist;
                    pickedAlbums.Add(album!);
                }
            }

            return pickedAlbums;
        }

        /// <summary>
        /// Build a randomised playlist one entry at a time accounting for:
        /// 
        /// 1. Style match to time of day ideal
        /// 2. Flow from the previous pick (modelled as a transition penalty)
        /// 3. Variety / anti-repetition
        /// 4. An element of randomness (top-K, Softmax temperature, random jitter))
        /// 
        /// </summary>
        /// <param name="artists"></param>
        /// <param name="timeOfDay"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static List<PlaylistArtist> BuildPlaylist(IEnumerable<Artist> artists, PlaylistParameters parameters)
        {
            // Materialize the artists list
            var artistList = artists.ToList();
            if (artistList.Count == 0)
            {
                return [];
            }

            // Create a random number generator - providing a seed produces repeatable playlists
            var rng = parameters.Seed != null ? new Random(parameters.Seed.Value) : new Random();

            // Build a per-artist mood score for the current time of day
            var moodScores = artists
                .SelectMany(a => a.Moods.Select(am => new { a.Id, am.Mood }))
                .GroupBy(x => x.Id)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => GetMoodWeight(x.Mood!, parameters.TimeOfDay))
                );

            // Compute style fit mood score for each artist
            var artistScores = new List<ArtistScoringRow>(artistList.Count);
            foreach (var a in artistList)
            {
                // Get the mood score and compute the style fit
                double moodScore = moodScores.TryGetValue(a.Id, out var score) ? score : 0.0;
                double styleFit = ComputeStyleFit(a.Energy, a.Intimacy, a.Warmth, parameters.TimeOfDay);

                // Create a new score for the artist and add it to the collection
                artistScores.Add(new ArtistScoringRow
                {
                    Artist = a,
                    MoodScore = moodScore,
                    StyleFit = styleFit
                });
            }

            // Normalize mood score to 0..1 for fair blending with style fit
            double minMood = artistScores.Min(r => r.MoodScore);
            double maxMood = artistScores.Max(r => r.MoodScore);
            foreach (var r in artistScores)
            {
                r.MoodScoreNorm = (maxMood > minMood)
                    ? (r.MoodScore - minMood) / (maxMood - minMood)
                    : 0.0;
            }

            // Compute a weighted base score with a small amount of jitter
            foreach (var row in artistScores)
            {
                row.BaseScore = (parameters.StyleWeight * row.StyleFit) +
                                (parameters.MoodWeight * row.MoodScoreNorm) +
                                NextGaussian(rng, 0.0, parameters.RandomJitter);
            }

            // Calculate the style vector for each
            var artistStyleVector = artistScores!.ToDictionary(
                r => r.Artist!.Id,
                r => (E: r.Artist!.Energy, I: r.Artist!.Intimacy, W: r.Artist!.Warmth)
            );

            // Iterative selection with top-K softmax sampling
            var chosen = new List<PlaylistArtist>(Math.Min(parameters.NumberOfEntries, artistScores!.Count));
            var remaining = new List<ArtistScoringRow>(artistScores);
            var recent = new Queue<int>();
            int? previousArtistId = null;

            while (chosen.Count < Math.Min(parameters.NumberOfEntries, remaining.Count))
            {
                // Calculate a score for each of the remaining artists
                var scoredRows = remaining.Select(r =>
                {
                    // Calculate penalties or costs for choosing this artist next and use them to calculate
                    // a score for the transition
                    double recentCost = recent.Contains(r.Artist!.Id) ? 1.0 : 0.0;
                    double transitionCost = CalculateTransitionCost(artistStyleVector, previousArtistId, r.Artist.Id);
                    double score = r.BaseScore
                                    - (parameters.TransitionPenalty * transitionCost)
                                    - (0.50 * recentCost);

                    return (Row: r, StepScore: score);
                })
                .OrderByDescending(x => x.StepScore)
                .ToList();

                // Take the top "K" entries from the scored list to form the pool of candidates to take forward
                bool isFirstPick = !previousArtistId.HasValue;

                // First pick: Widen the pool a bit so we don't always start from the same "magnet" artists.
                // After the first pick: revert to the normal curated tightness.
                int k = Math.Min(
                    isFirstPick ? Math.Min(scoredRows.Count, parameters.TopK + 4) : parameters.TopK,
                    scoredRows.Count
                );

                var pool = scoredRows.Take(k).ToList();

                // First pick: Slightly higher temperature so we explore within that wider pool
                double temperature = isFirstPick
                    ? NumberRangeExtensions.Clamp(parameters.Temperature * 1.35, 0.30, 1.15)
                    : parameters.Temperature;

                // Use Softmax to pick an entry from the pool
                var probs = Softmax([.. pool.Select(p => p.StepScore)], temperature);
                int pickIdx = SampleIndex(rng, probs);
                var (Row, StepScore) = pool[pickIdx];

                // Add a new artist playlist item. This includes the pick and the calculated values
                // that illustrate why it was picked 
                var artist = Row.Artist;
                chosen.Add(new PlaylistArtist
                {
                    ArtistId = artist!.Id,
                    ArtistName = artist.Name,
                    StepScore = StepScore,
                    BaseScore = Row.BaseScore,
                    StyleFit = Row.StyleFit,
                    MoodScore = Row.MoodScore,
                    MoodScoreNorm = Row.MoodScoreNorm,
                    Energy = artist.Energy,
                    Intimacy = artist.Intimacy,
                    Warmth = artist.Warmth,
                    VocalPresence = artist.Vocals,
                    EnsembleType = artist.Ensemble
                });

                // Update state for the next iteration
                previousArtistId = artist.Id;
                recent.Enqueue(artist.Id);
                while (recent.Count > parameters.AvoidRecent)
                {
                    recent.Dequeue();
                }

                // Remove the picked artist
                remaining.Remove(Row);
            }

            return chosen;
        }

        /// <summary>
        /// Calculate the transition cost from one artist to another
        /// </summary>
        /// <param name="artistStyleVector"></param>
        /// <param name="prevArtistId"></param>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        private static double CalculateTransitionCost(Dictionary<int, (int, int, int)> artistStyleVector, int? prevArtistId, int candidateId)
        {
            // If there's no previous artist, there's no transition cose
            if (prevArtistId == null)
            {
                return 0.0;
            }

            // Get the style properties for the previous and current candidate
            var (Ep, Ip, Wp) = artistStyleVector[prevArtistId.Value];
            var (Ec, Ic, Wc) = artistStyleVector[candidateId];

            // Calculate the distances between them
            double de = Ep - Ec;
            double di = Ip - Ic;
            double dw = Wp - Wc;

            // Calculate the Euclidean distance between them and normalise by the maximum possible
            // distance
            double dist = Math.Sqrt(de * de + di * di + dw * dw);
            double maximumDistance = Math.Sqrt(3 * MaximumStyleValue * MaximumStyleValue);
            return dist / maximumDistance;
        }

        /// <summary>
        /// Get the time of day weighting for a mood
        /// </summary>
        /// <param name="mood"></param>
        /// <param name="timeOfDay"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static double GetMoodWeight(Mood mood, TimeOfDay timeOfDay)
            => timeOfDay switch
            {
                TimeOfDay.Morning   => mood.MorningWeight,
                TimeOfDay.Afternoon => mood.AfternoonWeight,
                TimeOfDay.Evening   => mood.EveningWeight,
                TimeOfDay.Late      => mood.LateWeight,
                _ => throw new ArgumentOutOfRangeException(nameof(timeOfDay))
            };

        /// <summary>
        /// Calculate a normalized Euclidean similarity measuring how well a candidate’s energy, intimacy
        /// and warmth matches the time-of-day style target
        /// </summary>
        /// <param name="energy"></param>
        /// <param name="intimacy"></param>
        /// <param name="warmth"></param>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        private static double ComputeStyleFit(double energy, double intimacy, double warmth, TimeOfDay targetTime)
        {
            // Get the target point in 3D (E,I,W) space
            var (E, I, W) = _styleTargets[targetTime];

            // Calculate the vector from the target to the supplied point
            double de = energy - E;
            double di = intimacy - I;
            double dw = warmth - W;

            // Calculate the Euclidean distance between the target point and supplied point and the maximum
            // possible distance if they were as far away as possible
            double distance = Math.Sqrt(de * de + di * di + dw * dw);
            double maximumDistance = Math.Sqrt(3 * MaximumStyleValue * MaximumStyleValue);

            // Normalise the distance to a similarity between 0 and 1
            double fit = 1.0 - (distance / maximumDistance);
            return NumberRangeExtensions.Clamp(fit, 0.0, 1.0);
        }

        /// <summary>
        /// Given a set of scores, convert them into probabilities that sum to 1.0
        /// </summary>
        /// <param name="scores"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        private static double[] Softmax(double[] scores, double temperature)
        {
            // Prevent it blowing up if temperature is very small or 0
            temperature = Math.Max(temperature, 1e-6);

            double maximum = scores.Max();
            var exponentials = new double[scores.Length];
            double sum = 0.0;

            for (int i = 0; i < scores.Length; i++)
            {
                // This function uses exponentials and these can blow up very quickly. So, shift the
                // original scores by subtract the maximum before calculating the exponential. To
                // illustrate with an example, suppose T is 1:
                //
                // Original Scores: [10, 7, 3]
                // Shifted scores:  [0, -3, -7]
                // Exponentials:    [1, 0.05, 0.0009]
                //
                // The shift prevents possible overflow errors
                double x = (scores[i] - maximum) / temperature;
                double e = Math.Exp(x);
                exponentials[i] = e;
                sum += e;
            }

            // Normalise the results to probabilities in the range 0-1
            exponentials = [.. exponentials.Select(x => x/sum)];

            return exponentials;
        }

        /// <summary>
        /// Pick a random index for a probability in the probability list, where larger probabilities
        /// are more likely to be picked:
        /// 
        /// SCALE:      0 |----|----------|--------------------------------| 1
        /// PROBABILITY:    0.1     0.3                   0.7
        /// INDEX:           0       1                     2
        /// 
        /// The function walks the line until a random number in the range 0-1 falls in the cumulative
        /// probability so far, then returns the index at that point
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        private static int SampleIndex(Random rng, double[] probabilities)
        {
            // Pick a random number
            double r = rng.NextDouble();

            // Walk the array of probabilities calculating the cumulative probability until
            // our original random number falls in the current "slice"
            double cumulative = 0.0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (r <= cumulative)
                {
                    return i;
                }
            }

            // Fallback if we hit the end of the list
            return probabilities.Length - 1;
        }

        /// <summary>
        /// Random number generator that returns random numbers following a bell-curve (Gaussian)
        /// distribution using the Box-Muller transformation
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="mean"></param>
        /// <param name="stdDev"></param>
        /// <returns></returns>
        private static double NextGaussian(Random rng, double mean, double stdDev)
        {
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }
    }
}