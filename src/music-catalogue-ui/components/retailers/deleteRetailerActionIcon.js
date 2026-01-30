import { useCallback } from "react";
import {
  apiDeleteRetailer,
  apiFetchRetailers,
} from "@/helpers/api/apiRetailers";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to delete a retailer
 * @param {*} retailer
 * @param {*} logout
 * @param {*} clearError
 * @param {*} setError
 * @param {*} setRetailers
 * @returns
 */
const DeleteRetailerActionIcon = ({
  retailer,
  logout,
  clearError,
  setError,
  setRetailers,
}) => {
  /* Callback to prompt for confirmation and delete a retailer */
  const confirmDeleteRetailer = useCallback(
    async (e, album) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear any pre-existing error messages
      clearError();

      // Show a confirmation message and get the user response
      const message = `This will delete the retailer "${retailer.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the retailer and confirm the API call was successful
        const result = await apiDeleteRetailer(retailer.id, logout);
        if (result) {
          // Successful, so refresh the retailer list
          const fetchedRetailers = await apiFetchRetailers(logout);
          setRetailers(fetchedRetailers);
        } else {
          setError(`An error occurred deleting retailer "${retailer.name}"`);
        }
      }
    },
    [retailer, logout, clearError, setError, setRetailers]
  );

  return (
    <>
      <FontAwesomeIcon
        icon={faTrashAlt}
        data-tooltip-id="delete-tooltip"
        data-tooltip-content="Delete retailer"
        onClick={(e) => confirmDeleteRetailer(e, retailer)}
      />

      <Tooltip id="delete-tooltip" />
    </>
  );
};

export default DeleteRetailerActionIcon;
