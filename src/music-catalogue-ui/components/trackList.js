import { useCallback } from "react";
import useTracks from "@/hooks/useTracks";
import TrackRow from "./trackRow";
import pages from "@/helpers/navigation";
import ButtonBar from "./buttonBar";

/**
 * Component to render the list of tracks for the specified album
 * @param {*} param0
 * @returns
 */
const TrackList = ({ artist, album, navigate, logout }) => {
  const { tracks, setTracks } = useTracks(album.id, logout);

  // Backwards navigation callback
  const navigateBack = useCallback(() => {
    navigate(pages.albums, artist, null);
  }, [navigate, artist]);

  // Callback to navigate to the lookup page
  const lookup = useCallback(() => {
    navigate(pages.lookup, null, null);
  }, [navigate]);

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">
          {artist.name} - {album.title}
        </h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Album Title</th>
            <th>Artist</th>
            <th>No.</th>
            <th>Track</th>
            <th>Duration</th>
          </tr>
        </thead>
        <tbody>
          {tracks.map((t) => (
            <TrackRow
              key={t.id}
              artist={artist}
              album={album}
              track={t}
              navigate={navigate}
            />
          ))}
        </tbody>
      </table>
      <ButtonBar navigateBack={navigateBack} lookup={lookup} logout={logout} />
    </>
  );
};

export default TrackList;
