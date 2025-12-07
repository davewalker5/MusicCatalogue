using MusicCatalogue.Entities.Interfaces;
using MusicCatalogue.LookupTool.Interfaces;

namespace MusicCatalogue.LookupTool.Logic
{
    internal abstract class DataExportBase : IDataExporter
    {
        protected IMusicLogger Logger { get; private set; }
        protected IMusicCatalogueFactory Factory { get; private set; }

        protected DataExportBase(IMusicLogger logger, IMusicCatalogueFactory factory)
        {
            Logger = logger;
            Factory = factory;
        }

        /// <summary>
        /// Method to perform the export, supplied by the child class
        /// </summary>
        /// <param name="file"></param>
        public abstract void Export(string file);
    }
}
