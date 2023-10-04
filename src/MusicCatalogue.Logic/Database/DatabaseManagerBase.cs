using MusicCatalogue.Data;

namespace MusicCatalogue.Logic.Database
{
    public abstract class DatabaseManagerBase
    {
        protected readonly MusicCatalogueDbContext _context;

        protected DatabaseManagerBase(MusicCatalogueDbContext context)
        {
            _context = context;
        }
    }
}
