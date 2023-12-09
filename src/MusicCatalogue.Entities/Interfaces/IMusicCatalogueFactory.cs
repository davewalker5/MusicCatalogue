﻿using Microsoft.EntityFrameworkCore;
using MusicCatalogue.Entities.Reporting;

namespace MusicCatalogue.Entities.Interfaces
{
    public interface IMusicCatalogueFactory
    {
        DbContext Context { get; }
        IGenreManager Genres { get; }
        IAlbumManager Albums { get; }
        IArtistManager Artists { get; }
        ITrackManager Tracks { get; }
        IManufacturerManager Manufacturers { get; }
        IEquipmentTypeManager EquipmentTypes { get; }
        IEquipmentManager Equipment { get; }
        IRetailerManager Retailers { get; }
        IUserManager Users { get; }
        IImporter Importer { get; }
        ITrackExporter CatalogueCsvExporter { get; }
        ITrackExporter CatalogueXlsxExporter { get; }
        IEquipmentExporter EquipmentCsvExporter { get; }
        IEquipmentExporter EquipmentXlsxExporter { get; }
        IJobStatusManager JobStatuses { get; }
        ISearchManager Search { get; }
        IWishListBasedReport<GenreStatistics> GenreStatistics { get; }
        IWishListBasedReport<ArtistStatistics> ArtistStatistics { get; }
        IWishListBasedReport<MonthlySpend> MonthlySpend { get; }
        IWishListBasedReport<RetailerStatistics> RetailerStatistics { get; }
    }
}