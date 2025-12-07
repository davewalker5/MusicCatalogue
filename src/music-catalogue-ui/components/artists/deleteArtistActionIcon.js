import { useCallback } from "react";
import { apiDeleteArtist, apiFetchArtists } from "@/helpers/api/apiArtists";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";

/**
 * Icon and associated action to delete an artist
 * @param {*} filter
 * @param {*} genre
 * @param {*} artist
 * @param {*} isWishList
 * @param {*} logout
 * @param {*} setArtists
 * @param {*} setError
 * @returns
 */
const DeleteArtistActionIcon = ({
  filter,
  genre,
  artist,
  isWishList,
  logout,
  setArtists,
  setError,
}) => {
  /* Callback to prompt for confirmation and delete an artist */
  const confirmDeleteArtist = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the artist "${artist.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the artist and confirm the API call was successful
        try {
          const result = await apiDeleteArtist(artist.id, logout);
          if (result) {
            // Get the genre ID
            const genreId = genre != null ? genre.id : null;

            // Successful, so refresh the artist list
            const fetchedArtists = await apiFetchArtists(
              filter,
              genreId,
              isWishList,
              true,
              logout
            );
            setArtists(fetchedArtists);
          }
        } catch (ex) {
          setError(`Error deleting the artist: ${ex.message}`);
        }
      }
    },
    [filter, artist, genre, isWishList, logout, setArtists, setError]
  );

  return (
    <FontAwesomeIcon
      icon={faTrashAlt}
      onClick={(e) => confirmDeleteArtist(e)}
    />
  );
};

export default DeleteArtistActionIcon;
