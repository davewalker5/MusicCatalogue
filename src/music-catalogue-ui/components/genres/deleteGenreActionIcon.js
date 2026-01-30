import { useCallback } from "react";
import { apiDeleteGenre, apiFetchGenres } from "@/helpers/api/apiGenres";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to delete a genre
 * @param {*} genre
 * @param {*} logout
 * @param {*} setGenres
 * @param {*} setError
 * @returns
 */
const DeleteGenreActionIcon = ({ genre, logout, setGenres, setError }) => {
  /* Callback to prompt for confirmation and delete a genre */
  const confirmDeleteGenre = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the genre "${genre.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        try {
          // ... delete the genre and confirm the API call was successful
          const result = await apiDeleteGenre(genre.id, logout);
          if (result) {
            // Successful, so refresh the genre list
            const fetchedGenres = await apiFetchGenres(null, logout);
            setGenres(fetchedGenres);
          } else {
            setError(`Failed to delete genre ${genre.name}`);
          }
        } catch (ex) {
          setError(`Error deleting the genre: ${ex.message}`);
        }
      }
    },
    [genre, logout, setGenres, setError]
  );

  return (
    <>
      <FontAwesomeIcon
        icon={faTrashAlt}
        data-tooltip-id="delete-tooltip"
        data-tooltip-content="Delete genre"
        onClick={(e) => confirmDeleteGenre(e)}
      />

      <Tooltip id="delete-tooltip" />
    </>
  );
};

export default DeleteGenreActionIcon;
