import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import {
  apiDeleteEquipmentType,
  apiFetchEquipmentTypes,
} from "@/helpers/api/apiEquipmentTypes";

/**
 * Icon and associated action to delete an equipment type
 * @param {*} equipmentType
 * @param {*} logout
 * @param {*} setEquipmentTypes
 * @param {*} setError
 * @returns
 */
const DeleteEquipmentTypeActionIcon = ({
  equipmentType,
  logout,
  setEquipmentTypes,
  setError,
}) => {
  /* Callback to prompt for confirmation and delete an equipment type */
  const confirmDeleteEquipmentType = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the equipment type "${equipmentType.name}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the equipment type and confirm the API call was successful
        try {
          const result = await apiDeleteEquipmentType(equipmentType.id, logout);
          if (result) {
            // Successful, so refresh the equipment type list
            const fetchedEquipmentTypes = await apiFetchEquipmentTypes(logout);
            setEquipmentTypes(fetchedEquipmentTypes);
          }
        } catch (ex) {
          setError(`Error deleting the equipment type: ${ex.message}`);
        }
      }
    },
    [equipmentType, logout, setEquipmentTypes, setError]
  );

  return (
    <FontAwesomeIcon
      icon={faTrashAlt}
      onClick={(e) => confirmDeleteEquipmentType(e)}
    />
  );
};

export default DeleteEquipmentTypeActionIcon;
