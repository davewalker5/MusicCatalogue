import { useCallback } from "react";
import { apiDeleteAlbum, apiFetchAlbumsByArtist } from "@/helpers/api";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";

const DeleteAlbum = ({ album, isWishList, logout, setAlbums }) => {
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
    [isWishList, logout, setAlbums]
  );

  return (
    <FontAwesomeIcon
      icon={faTrashAlt}
      onClick={(e) => confirmDeleteAlbum(e, album)}
    />
  );
};

export default DeleteAlbum;
