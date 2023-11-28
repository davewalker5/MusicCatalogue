using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Logic.Database
{
    public class SearchManager : DatabaseManagerBase, ISearchManager
    {
        private const string Wildcard = "*";

        internal SearchManager(IMusicCatalogueFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Return the artists matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<List<Artist>?> ArtistSearchAsync(ArtistSearchCriteria criteria)
        {
            List<Artist>? artists = null;

            // Replace a wildcard artist name prefix with null
            var prefix = criteria.NamePrefix == Wildcard ? null : criteria.NamePrefix;

            // Although we're returning a list of artists, the filtering criteria tend to refer to the
            // albums by that artist. For instance, the query "return artists in the Jazz genre" translates
            // as "return artists who have produced an album in the Jazz genre". So, start by retrieving a
            // list of albums matching the criteria then derive the artists from that
            var albums = await Factory.Albums
                                      .ListAsync(x => (
                                                            (criteria.WishList == null) ||
                                                            ((criteria.WishList == false) && (x.IsWishListItem == null)) ||
                                                            (x.IsWishListItem == criteria.WishList)
                                                      ) &&
                                                      (
                                                            (criteria.GenreId == null) ||
                                                            (x.GenreId == criteria.GenreId)
                                                      ));

            // If there are no albums, there can't be any matching artists
            if (albums.Any())
            {
                // Compile a list of unique artist IDs and load the matching artists
                var artistIds = albums.Select(x => x.ArtistId).Distinct().ToList();
                artists = await Factory.Artists
                                       .ListAsync(x => artistIds.Contains(x.Id) &&
                                                       (
                                                            (prefix == null) ||
                                                            ((x.SearchableName != null) && x.SearchableName.StartsWith(prefix)) ||
                                                            ((x.SearchableName == null) && x.Name.StartsWith(prefix))
                                                       ),
                                                  false);

                // Now map the albums onto their associated artists
                foreach (var artist in artists)
                {
                    artist.Albums = albums.Where(x => x.ArtistId == artist.Id).ToList();
                }
            }

            // If requested, include artists with no albums
            if (criteria.IncludeArtistsWithNoAlbums ?? false)
            {
                // Find artists with no albums that match the artist filters 
                var artistsWithNoAlbums = await Factory.Artists
                                                       .ListAsync(x => !x.Albums!.Any() &&
                                                       (
                                                            (prefix == null) ||
                                                            ((x.SearchableName != null) && x.SearchableName.StartsWith(prefix)) ||
                                                            ((x.SearchableName == null) && x.Name.StartsWith(prefix))
                                                       ),
                                                  true);

                // If there are any, add them to the returned collection
                if (artistsWithNoAlbums.Any())
                {
                    artists!.AddRange(artistsWithNoAlbums);
                }
            }

            return artists;
        }

        /// <summary>
        /// Return the albums matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<List<Album>?> AlbumSearchAsync(AlbumSearchCriteria criteria)
        {
            // Retrieve a list of matching albums
            var albums = await Factory.Albums
                                       .ListAsync(x => (
                                                            (criteria.WishList == null) ||
                                                            ((criteria.WishList == false) && (x.IsWishListItem == null)) ||
                                                            (x.IsWishListItem == criteria.WishList)
                                                       ) &&
                                                       (
                                                            (criteria.ArtistId == null) ||
                                                            (x.ArtistId == criteria.ArtistId)
                                                       ) &&
                                                       (
                                                            (criteria.GenreId == null) ||
                                                            (x.GenreId == criteria.GenreId)
                                                       ));

            return albums.Any() ? albums : null;
        }

        /// <summary>
        /// Return the genres matching the specified criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<List<Genre>?> GenreSearchAsync(GenreSearchCriteria criteria)
        {
            List<Genre>? genres = null;

            // Retrieve a list of albums, as they're the entity that's tagged with a genre
            var albums = await Factory.Albums
                                      .ListAsync(x => (
                                                            (criteria.WishList == null) ||
                                                            ((criteria.WishList == false) && (x.IsWishListItem == null)) ||
                                                            (x.IsWishListItem == criteria.WishList)
                                                      ));

            // If there are no albums, there can't be any matching artists
            if (albums.Any())
            {
                // Get a list of unique genre IDs and load the matching genres
                var genreIds = albums.Select(x => x.GenreId).Distinct().ToList();
                genres = await Factory.Genres.ListAsync(x => genreIds.Contains(x.Id));
            }

            return genres;
        }
    }
}
