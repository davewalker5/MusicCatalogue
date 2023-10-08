using MusicCatalogue.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MusicCatalogue.Logic.DataExchange
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
