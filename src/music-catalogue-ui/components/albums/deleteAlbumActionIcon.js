import { useCallback } from "react";
import {
  apiDeleteAlbum,
  apiFetchAlbumsByArtist,
} from "@/helpers/api/apiAlbums";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";

/**
 * Icon and associated action to delete an album
 * @param {*} album
 * @param {*} isWishList
 * @param {*} logout
 * @param {*} setAlbums
 * @param {*} setError
 * @returns
 */
const DeleteAlbumActionIcon = ({
  album,
  isWishList,
  logout,
  setAlbums,
  setError,
}) => {
  /* Callback to prompt for confirmation and delete an album */
  const confirmDeleteAlbum = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the album "${album.title}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        try {
          // ... delete the album and confirm the API call was successful
          const result = await apiDeleteAlbum(album.id, logout);
          if (result) {
            // Successful, so refresh the album list
            const fetchedAlbums = await apiFetchAlbumsByArtist(
              album.artistId,
              isWishList,
              logout
            );
            setAlbums(fetchedAlbums);
          } else {
            setError(`Failed to delete album ${album.title}`);
          }
        } catch (ex) {
          setError(`Error deleting the album: ${ex.message}`);
        }
      }
    },
    [album, isWishList, logout, setAlbums, setError]
  );

  return (
    <FontAwesomeIcon icon={faTrashAlt} onClick={(e) => confirmDeleteAlbum(e)} />
  );
};

export default DeleteAlbumActionIcon;
