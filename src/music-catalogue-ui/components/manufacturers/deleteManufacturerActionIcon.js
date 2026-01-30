import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import {
  apiDeleteManufacturer,
  apiFetchManufacturers,
} from "@/helpers/api/apiManufacturers";
import { Tooltip } from "react-tooltip";

/**
 * Icon and associated action to delete a manufacturer
 * @param {*} manufacturer
 * @param {*} logout
 * @param {*} setManufacturers
 * @param {*} setError
 * @returns
 */
const DeleteManufacturerActionIcon = ({
  manufacturer,
  logout,
  setManufacturers,
  setError,
}) => {
  /* Callback to prompt for confirmation and delete avmanufacturer */
  const confirmDeleteManufacturer = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the manufacturer "${manufacturer.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the manufacturer and confirm the API call was successful
        try {
          const result = await apiDeleteManufacturer(manufacturer.id, logout);
          if (result) {
            // Successful, so refresh the manufacturer list
            const fetchedManufacturers = await apiFetchManufacturers(logout);
            setManufacturers(fetchedManufacturers);
          }
        } catch (ex) {
          setError(`Error deleting the manufacturer: ${ex.message}`);
        }
      }
    },
    [manufacturer, logout, setManufacturers, setError]
  );

  return (
    <>
      <FontAwesomeIcon
        icon={faTrashAlt}
        data-tooltip-id="delete-tooltip"
        data-tooltip-content="Delete manufacturer"
        onClick={(e) => confirmDeleteManufacturer(e)}
      />

      <Tooltip id="delete-tooltip" />
    </>
  );
};

export default DeleteManufacturerActionIcon;
