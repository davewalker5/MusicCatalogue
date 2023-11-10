import { useCallback } from "react";
import useAlbums from "@/hooks/useAlbums";
import AlbumRow from "./albumRow";
import { apiDeleteAlbum, apiFetchAlbumsByArtist } from "@/helpers/api";

/**
 * Component to render the table of all albums by the specified artist
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const AlbumList = ({ artist, isWishList, navigate, logout }) => {
  const { albums, setAlbums } = useAlbums(artist.id, isWishList, logout);

  // Set the page title to reflect whether we're viewing the wish list
  const title = isWishList
    ? `Wish List for ${artist.name}`
    : `Albums by ${artist.name}`;

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
          const fetchedAlbums = await apiFetchAlbumsByArtist(
            artist.id,
            isWishList,
            logout
          );
          setAlbums(fetchedAlbums);
        }
      }
    },
    [artist, isWishList, setAlbums, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
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
              isWishList={isWishList}
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
