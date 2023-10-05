import pages from "../helpers/navigation";

const ComponentPicker = ({ currentNavLocation }) => {
  switch (currentNavLocation) {
    case pages.artists:
      return <span></span>;
    case pages.albums:
      return <span></span>;
    case pages.tracks:
      return <span></span>;
    default:
      return <span></span>;
  }
};

export default ComponentPicker;
