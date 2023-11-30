import pages from "@/helpers/navigation";
import ArtistList from "./artistList";
import AlbumList from "./albumList";
import TrackList from "./trackList";
import LookupAlbum from "./lookupAlbum";
import ExportCatalogue from "./exportCatalogue";
import GenreStatusReport from "./genreStatisticsReport";
import JobStatusReport from "./jobStatusReport";
import AlbumPurchaseDetails from "./albumPurchaseDetails";
import ArtistStatisticsReport from "./artistStatisticsReport";
import MonthlySpendReport from "./monthlySpendReport";
import GenreList from "./genreList";
import RetailerList from "./retailerList";
import RetailerDetails from "./retailerDetails";
import RetailerEditor from "./retailerEditor";
import TrackEditor from "./trackEditor";
import AlbumEditor from "./albumEditor";
import ArtistEditor from "./artistEditor";
import RetailerStatisticsReport from "./retailerStatisticsReport";

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
