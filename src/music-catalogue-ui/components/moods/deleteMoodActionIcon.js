import { useCallback } from "react";
import { apiDeleteMood, apiFetchMoods } from "@/helpers/api/apiMoods";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to delete a mood
 * @param {*} mood
 * @param {*} logout
 * @param {*} setMoods
 * @param {*} setError
 * @returns
 */
const DeleteMoodActionIcon = ({ mood, logout, setMoods, setError }) => {
  /* Callback to prompt for confirmation and delete a mood */
  const confirmDeleteMood = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the mood "${mood.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        try {
          // ... delete the mood and confirm the API call was successful
          const result = await apiDeleteMood(mood.id, logout);
          if (result) {
            // Successful, so refresh the mood list
            const fetchedMoods = await apiFetchMoods(logout);
            setMoods(fetchedMoods);
          } else {
            setError(`Failed to delete mood ${mood.name}`);
          }
        } catch (ex) {
          setError(`Error deleting the mood: ${ex.message}`);
        }
      }
    },
    [mood, logout, setMoods, setError]
  );

  return (
    <>
      <FontAwesomeIcon
        icon={faTrashAlt}
        data-tooltip-id="delete-tooltip"
        data-tooltip-content="Delete mood"
        onClick={(e) => confirmDeleteMood(e)}
      />

      <Tooltip id="delete-tooltip" />
    </>
  );
};

export default DeleteMoodActionIcon;
