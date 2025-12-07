using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.Logic.Database
{
    public abstract class DatabaseManagerBase
    {
        protected IMusicCatalogueFactory Factory { get; private set; }
        protected MusicCatalogueDbContext Context { get { return (Factory.Context as MusicCatalogueDbContext)!; } }

        protected DatabaseManagerBase(IMusicCatalogueFactory factory)
        {
            Factory = factory;
        }
    }
}
