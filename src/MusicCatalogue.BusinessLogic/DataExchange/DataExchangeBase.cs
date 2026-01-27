using MusicCatalogue.Entities.Interfaces;

namespace MusicCatalogue.BusinessLogic.DataExchange
{
    public abstract class DataExchangeBase
    {
        protected readonly IMusicCatalogueFactory _factory;

        protected DataExchangeBase(IMusicCatalogueFactory factory)
        {
            _factory = factory;
        }
    }
}
