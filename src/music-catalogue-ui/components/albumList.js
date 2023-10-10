import useAlbums from "@/hooks/useAlbums";
import statuses from "@/helpers/status";
import AlbumRow from "./albumRow";
import StatusIndicator from "./statusIndicator";
import pages from "@/helpers/navigation";

const AlbumList = ({ artist, navigate }) => {
  const { albums, setAlbums, currentStatus } = useAlbums(artist.id);

  if (currentStatus !== statuses.loaded)
    return <StatusIndicator currentStatus={currentStatus} />;

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">Albums by {artist.name}</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Album Title</th>
            <th>Artist</th>
            <th>Genre</th>
            <th>Released</th>
          </tr>
        </thead>
        <tbody>
          {albums.map((a) => (
            <AlbumRow
              key={a.id}
              artist={artist}
              album={a}
              navigate={navigate}
            />
          ))}
        </tbody>
      </table>
      <button
        className="btn btn-primary"
        onClick={() => navigate(pages.artists, null, null)}
      >
        &lt; Back
      </button>
    </>
  );
};

export default AlbumList;
