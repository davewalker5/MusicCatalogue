﻿using MusicCatalogue.Entities.Database;
using MusicCatalogue.Entities.Search;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface ISearchManager
    {
        Task<List<Album>?> AlbumSearchAsync(AlbumSearchCriteria criteria);
        Task<List<Artist>?> ArtistSearchAsync(ArtistSearchCriteria criteria);
    }
}