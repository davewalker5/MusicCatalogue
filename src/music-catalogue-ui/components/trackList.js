import useTracks from "@/hooks/useTracks";
import statuses from "@/helpers/status";
import TrackRow from "./trackRow";
import StatusIndicator from "./statusIndicator";

const TrackList = ({ artist, album }) => {
  const { tracks, setTracks, currentStatus } = useTracks(album.id);

  if (currentStatus !== statuses.loaded)
    return <StatusIndicator currentStatus={currentStatus} />;

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">Tracks</h5>
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
            <TrackRow key={t.id} artist={artist} album={album} track={t} />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default TrackList;
