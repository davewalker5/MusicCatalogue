import pages from "@/helpers/navigation";
import ArtistList from "./artists/artistList";
import AlbumList from "./albums/albumList";
import TrackList from "./tracks/trackList";
import LookupAlbum from "./search/lookupAlbum";
import ExportCatalogue from "./dataexchange/exportCatalogue";
import GenreStatusReport from "./reports/genreStatisticsReport";
import JobStatusReport from "./reports/jobStatusReport";
import AlbumPurchaseDetails from "./albums/albumPurchaseDetails";
import ArtistStatisticsReport from "./reports/artistStatisticsReport";
import MonthlySpendReport from "./reports/monthlySpendReport";
import GenreList from "./genres/genreList";
import RetailerList from "./retailers/retailerList";
import RetailerDetails from "./retailers/retailerDetails";
import RetailerEditor from "./retailers/retailerEditor";
import TrackEditor from "./tracks/trackEditor";
import AlbumEditor from "./albums/albumEditor";
import ArtistEditor from "./artists/artistEditor";
import RetailerStatisticsReport from "./reports/retailerStatisticsReport";
import EquipmentTypeList from "./equipmentTypes/equipmentTypeList";
import EquipmentTypeEditor from "./equipmentTypes/equipmentTypeEditor";

/**
 * Component using the current context to select and render the current page
 * @param {*} context
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ComponentPicker = ({ context, navigate, logout }) => {
  switch (context.page) {
    case pages.artists:
      return (
        <ArtistList
          filter={context.filter}
          genre={context.genre}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.artistEditor:
      return (
        <ArtistEditor
          filter={context.filter}
          artist={context.artist}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albums:
      return (
        <AlbumList
          artist={context.artist}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albumEditor:
      return (
        <AlbumEditor
          artist={context.artist}
          album={context.album}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albumPurchaseDetails:
      return (
        <AlbumPurchaseDetails
          artist={context.artist}
          album={context.album}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.tracks:
      return (
        <TrackList
          artist={context.artist}
          album={context.album}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.trackEditor:
      return (
        <TrackEditor
          track={context.track}
          artist={context.artist}
          album={context.album}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.lookup:
      return <LookupAlbum navigate={navigate} logout={logout} />;
    case pages.retailers:
      return <RetailerList navigate={navigate} logout={logout} />;
    case pages.retailerDetails:
      return (
        <RetailerDetails
          retailer={context.retailer}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.retailerEditor:
      return (
        <RetailerEditor
          retailer={context.retailer}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.genres:
      return <GenreList navigate={navigate} logout={logout} />;
    case pages.equipmentTypes:
      return <EquipmentTypeList navigate={navigate} logout={logout} />;
    case pages.equipmentTypeEditor:
      return (
        <EquipmentTypeEditor
          equipmentType={context.equipmentType}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.export:
      return <ExportCatalogue logout={logout} />;
    case pages.artistStatisticsReport:
      return <ArtistStatisticsReport logout={logout} />;
    case pages.genreStatisticsReport:
      return <GenreStatusReport logout={logout} />;
    case pages.retailerStatisticsReport:
      return <RetailerStatisticsReport logout={logout} />;
    case pages.jobStatusReport:
      return <JobStatusReport logout={logout} />;
    case pages.monthlySpendReport:
      return <MonthlySpendReport logout={logout} />;
    default:
      return <span />;
  }
};

export default ComponentPicker;
