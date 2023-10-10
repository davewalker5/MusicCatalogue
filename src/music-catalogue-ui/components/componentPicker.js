import pages from "../helpers/navigation";
import ArtistList from "./artistList";
import AlbumList from "./albumList";
import TrackList from "./trackList";

const ComponentPicker = ({ context, navigate }) => {
  switch (context.page) {
    case pages.artists:
      return <ArtistList navigate={navigate} />;
    case pages.albums:
      return <AlbumList artist={context.artist} navigate={navigate} />;
    case pages.tracks:
      return <TrackList artist={context.artist} album={context.album} />;
    default:
      return <span></span>;
  }
};

export default ComponentPicker;
