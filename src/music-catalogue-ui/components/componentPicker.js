import pages from "../helpers/navigation";
import ArtistList from "./artistList";
import AlbumList from "./albumList";

const ComponentPicker = ({ context, navigate }) => {
  switch (context.page) {
    case pages.artists:
      return <ArtistList navigate={navigate} />;
    case pages.albums:
      return <AlbumList artist={context.artist} navigate={navigate} />;
    case pages.tracks:
      return <span></span>;
    default:
      return <span></span>;
  }
};

export default ComponentPicker;
