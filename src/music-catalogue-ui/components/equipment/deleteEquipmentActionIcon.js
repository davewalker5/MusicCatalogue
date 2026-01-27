import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrashAlt } from "@fortawesome/free-solid-svg-icons";
import {
  apiDeleteEquipment,
  apiFetchEquipment,
} from "@/helpers/api/apiEquipment";

/**
 * Icon and associated action to delete an item of equipment
 * @param {*} equipment
 * @param {*} isWishList
 * @param {*} logout
 * @param {*} setEquipment
 * @param {*} setError
 * @returns
 */
const DeleteEquipmentActionIcon = ({
  equipment,
  isWishList,
  logout,
  setEquipment,
  setError,
}) => {
  /* Callback to prompt for confirmation and delete an item of equipment */
  const confirmDeleteEquipment = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Show a confirmation message and get the user response
      const message = `This will delete the equipment "${equipment.description}" - click OK to confirm`;
      const result = confirm(message);

      // If they've confirmed the deletion ...
      if (result) {
        // ... delete the equipment and confirm the API call was successful
        try {
          const result = await apiDeleteEquipment(equipment.id, logout);
          if (result) {
            // Successful, so refresh the equipment list
            const fetchedEquipment = await apiFetchEquipment(
              isWishList,
              logout
            );
            setEquipment(fetchedEquipment);
          }
        } catch (ex) {
          setError(`Error deleting the equipment: ${ex.message}`);
        }
      }
    },
    [equipment, isWishList, logout, setEquipment, setError]
  );

  return (
    <FontAwesomeIcon
      icon={faTrashAlt}
      onClick={(e) => confirmDeleteEquipment(e)}
    />
  );
};

export default DeleteEquipmentActionIcon;
