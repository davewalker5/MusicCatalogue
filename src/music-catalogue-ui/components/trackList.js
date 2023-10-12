import { useCallback } from "react";
import useTracks from "@/hooks/useTracks";
import TrackRow from "./trackRow";
import pages from "@/helpers/navigation";

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

  return (
    <>
      <div className="row mb-2 pageTitle">
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
      <button className="btn btn-primary" onClick={() => navigateBack()}>
        &lt; Back to Albums By {artist.name}
      </button>
    </>
  );
};

export default TrackList;
