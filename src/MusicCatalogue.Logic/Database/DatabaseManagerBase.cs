using MusicCatalogue.Data;
using System.Globalization;

namespace MusicCatalogue.Logic.Database
{
    public abstract class DatabaseManagerBase
    {
        protected readonly TextInfo _textInfo = new CultureInfo("en-GB", false).TextInfo;
        protected readonly MusicCatalogueDbContext _context;

        public DatabaseManagerBase(MusicCatalogueDbContext context)
        {
            _context = context;
        }
    }
}
