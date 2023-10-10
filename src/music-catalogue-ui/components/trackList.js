import useTracks from "@/hooks/useTracks";
import statuses from "@/helpers/status";
import TrackRow from "./trackRow";
import StatusIndicator from "./statusIndicator";
import pages from "@/helpers/navigation";

const TrackList = ({ artist, album, navigate }) => {
  const { tracks, setTracks, currentStatus } = useTracks(album.id);

  if (currentStatus !== statuses.loaded)
    return <StatusIndicator currentStatus={currentStatus} />;

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
      <button
        className="btn btn-primary"
        onClick={() => navigate(pages.albums, artist, null)}
      >
        &lt; Back
      </button>
    </>
  );
};

export default TrackList;
