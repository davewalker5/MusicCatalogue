import pages from "../helpers/navigation";
import ArtistList from "./artistList";
import AlbumList from "./albumList";
import TrackList from "./trackList";
import LookupAlbum from "./lookupAlbum";
import ExportCatalogue from "./exportCatalogue";
import GenreStatusReport from "./genreStatisticsReport";
import JobStatusReport from "./jobStatusReport";
import AlbumPurchaseDetails from "./albumPurchaseDetails";
import ArtistStatisticsReport from "./artistStatisticsReport";

/**
 * Component using the current page name to render the components required
 * by that page
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
          filter={"A"}
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
    case pages.lookup:
      return <LookupAlbum navigate={navigate} logout={logout} />;
    case pages.export:
      return <ExportCatalogue logout={logout} />;
    case pages.artistStatisticsReport:
      return <ArtistStatisticsReport logout={logout} />;
    case pages.genreStatisticsReport:
      return <GenreStatusReport logout={logout} />;
    case pages.jobStatusReport:
      return <JobStatusReport logout={logout} />;
    case pages.albumPurchaseDetails:
      return (
        <AlbumPurchaseDetails
          artist={context.artist}
          album={context.album}
          navigate={navigate}
          logout={logout}
        />
      );
    default:
      return <span />;
  }
};

export default ComponentPicker;
