import { useCallback } from "react";
import useAlbums from "@/hooks/useAlbums";
import AlbumRow from "./albumRow";
import pages from "@/helpers/navigation";
import ButtonBar from "./buttonBar";

/**
 * Component to render the table of all albums by the specified artist
 * @param {*} param0
 * @returns
 */
const AlbumList = ({ artist, navigate, logout }) => {
  const { albums, setAlbums } = useAlbums(artist.id, logout);

  // Backwards navigation callback
  const navigateBack = useCallback(() => {
    navigate(pages.artists, null, null);
  }, [navigate]);

  // Callback to navigate to the lookup page
  const lookup = useCallback(() => {
    navigate(pages.lookup, null, null);
  }, [navigate]);

  return (
    <>
      <div className="row mb-2">
        <h5 className="themeFontColor text-center">Albums by {artist.name}</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Artist</th>
            <th>Album Title</th>
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
