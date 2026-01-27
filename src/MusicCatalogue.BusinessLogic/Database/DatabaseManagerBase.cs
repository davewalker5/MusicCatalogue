using MusicCatalogue.Data;
using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.Database
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
