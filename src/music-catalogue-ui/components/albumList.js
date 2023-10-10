import { useCallback } from "react";
import useAlbums from "@/hooks/useAlbums";
import statuses from "@/helpers/status";
import AlbumRow from "./albumRow";
import StatusIndicator from "./statusIndicator";
import pages from "@/helpers/navigation";
import ButtonBar from "./buttonBar";

const AlbumList = ({ artist, navigate, logout }) => {
  const { albums, setAlbums, currentStatus } = useAlbums(artist.id, logout);

  // Backwards navigation callback
  const navigateBack = useCallback(() => {
    navigate(pages.artists, null, null);
  }, [navigate]);

  // Callback to navigate to the lookup page
  const lookup = useCallback(() => {
    navigate(pages.lookup, null, null);
  }, [navigate]);

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
      <ButtonBar navigateBack={navigateBack} lookup={lookup} logout={logout} />
    </>
  );
};

export default AlbumList;
