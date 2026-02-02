using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Exceptions;
using MusicCatalogue.Entities.Interfaces;
using System.Linq.Expressions;

namespace MusicCatalogue.BusinessLogic.Database
{
    public class MoodManager : DatabaseManagerBase, IMoodManager
    {
        internal MoodManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the first mood matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Mood> GetAsync(Expression<Func<Mood, bool>> predicate)
        {
            List<Mood> moods = await ListAsync(predicate);

#pragma warning disable CS8603
            return moods.FirstOrDefault();
#pragma warning restore CS8603
        }

        /// <summary>
        /// Return all moods matching the specified criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<Mood>> ListAsync(Expression<Func<Mood, bool>> predicate)
            => await Context.Moods
                            .Where(predicate)
                            .OrderBy(x => x.Name)
                            .ToListAsync();

        /// <summary>
        /// Add a mood, if it doesn't already exist
        /// </summary>
        /// <param name="name"></param>
        /// <param name="morningWeight"></param>
        /// <param name="afternoonWeight"></param>
        /// <param name="eveningWeight"></param>
        /// <param name="lateWeight"></param>
        /// <returns></returns>
        public async Task<Mood> AddAsync(
            string name,
            double morningWeight,
            double afternoonWeight,
            double eveningWeight,
            double lateWeight)
        {
            var clean = StringCleaner.Clean(name)!;
            var mood = await GetAsync(a => a.Name == clean);

            if (mood == null)
            {
                // Get a serchable name
                mood = new Mood
                {
                    Name = clean,
                    MorningWeight = morningWeight,
                    AfternoonWeight = afternoonWeight,
                    EveningWeight = eveningWeight,
                    LateWeight = lateWeight
                };

                await Context.Moods.AddAsync(mood);
                await Context.SaveChangesAsync();
            }

            return mood;
        }

        /// <summary>
        /// Update the properties of the specified mood
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="morningWeight"></param>
        /// <param name="afternoonWeight"></param>
        /// <param name="eveningWeight"></param>
        /// <param name="lateWeight"></param>
        /// <returns></returns>
        public async Task<Mood?> UpdateAsync(
            int id,
            string name,
            double morningWeight,
            double afternoonWeight,
            double eveningWeight,
            double lateWeight)
        {
            var mood = Context.Moods.FirstOrDefault(x => x.Id == id);
            if (mood != null)
            {
                // Save the changes
                mood.Name = StringCleaner.Clean(name)!;
                mood.MorningWeight = morningWeight;
                mood.AfternoonWeight = afternoonWeight;
                mood.EveningWeight = eveningWeight;
                mood.LateWeight = lateWeight;
                await Context.SaveChangesAsync();
            }

            return mood;
        }

        /// <summary>
        /// Delete the mood with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the mood record and check it exists
            var mood = await GetAsync(x => x.Id == id);
            if (mood != null)
            {
                // If there are any artists associated with the mood, they can't be deleted
                var artistMoods = Context.ArtistMoods.Where(x => x.MoodId == id);
                if (artistMoods.Any())
                {
                    var message = $"Cannot delete mood '{mood.Name} with Id {id} with artists associated with it";
                    throw new MoodInUseException(message);
                }

                // Delete the mood record and save changes
                Factory.Context.Remove(mood);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}