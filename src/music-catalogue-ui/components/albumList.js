import { useCallback } from "react";
import useAlbums from "@/hooks/useAlbums";
import AlbumRow from "./albumRow";
import { apiDeleteAlbum, apiFetchAlbumsByArtist } from "@/helpers/api";

/**
 * Component to render the table of all albums by the specified artist
 * @param {*} param0
 * @returns
 */
const AlbumList = ({ artist, navigate, logout }) => {
  const { albums, setAlbums } = useAlbums(artist.id, logout);

  /* Callback to prompt for confirmation and delete an album */
  const confirmDeleteAlbum = useCallback(
    async (e, album) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the album "${album.title}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the album and confirm the API call was successful
        const result = await apiDeleteAlbum(album.id, logout);
        if (result) {
          // Successful, so refresh the album list
          const fetchedAlbums = await apiFetchAlbumsByArtist(artist.id, logout);
          setAlbums(fetchedAlbums);
        }
      }
    },
    [artist, setAlbums, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Albums by {artist.name}</h5>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Artist</th>
            <th>Album Title</th>
            <th>Genre</th>
            <th>Released</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {(albums ?? []).map((a) => (
            <AlbumRow
              key={a.id}
              id={a.id}
              artist={artist}
              album={a}
              navigate={navigate}
              deleteAlbum={confirmDeleteAlbum}
            />
          ))}
        </tbody>
      </table>
    </>
  );
};

export default AlbumList;
