using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.Database
{
    public class ArtistMoodManager : DatabaseManagerBase, IArtistMoodManager
    {
        internal ArtistMoodManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Add a mapping, if it doesn't already exist
        /// </summary>
        /// <param name="artistId"></param>
        /// <param name="moodId"></param>
        /// <returns></returns>
        public async Task<ArtistMood> AddAsync(int artistId, int moodId)
        {
            var mapping = Context.ArtistMoods.FirstOrDefault(m => (m.ArtistId == artistId) &&  (m.MoodId == moodId));
            if (mapping == null)
            {
                mapping = new ArtistMood
                {
                    ArtistId = artistId,
                    MoodId = moodId
                };

                await Context.ArtistMoods.AddAsync(mapping);
                await Context.SaveChangesAsync();
            }

            return mapping;
        }

        /// <summary>
        /// Delete the mapping with the specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            // Find the mapping record and check it exists
            var mapping = Context.ArtistMoods.FirstOrDefault(x => x.Id == id);
            if (mapping != null)
            {
                // Delete the mapping record and save changes
                Factory.Context.Remove(mapping);
                await Factory.Context.SaveChangesAsync();
            }
        }
    }
}