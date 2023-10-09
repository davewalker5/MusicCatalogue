import pages from "../helpers/navigation";
import ArtistList from "./artistList";

const ComponentPicker = ({ currentPage }) => {
  switch (currentPage) {
    case pages.artists:
      return <ArtistList />;
    case pages.albums:
      return <span></span>;
    case pages.tracks:
      return <span></span>;
    default:
      return <span></span>;
  }
};

export default ComponentPicker;
