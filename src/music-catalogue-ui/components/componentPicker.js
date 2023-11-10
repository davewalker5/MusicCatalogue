import pages from "../helpers/navigation";
import ArtistList from "./artistList";
import AlbumList from "./albumList";
import TrackList from "./trackList";
import LookupAlbum from "./lookupAlbum";
import ExportCatalogue from "./exportCatalogue";
import JobStatusReport from "./jobStatusReport";

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
    case pages.jobStatusReport:
      return <JobStatusReport logout={logout} />;
    default:
      return <span />;
  }
};

export default ComponentPicker;
