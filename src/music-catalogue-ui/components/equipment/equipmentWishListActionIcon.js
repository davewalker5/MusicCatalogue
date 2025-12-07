import { useCallback } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faHeartCirclePlus,
  faRecordVinyl,
} from "@fortawesome/free-solid-svg-icons";
import {
  apiFetchEquipment,
  apiSetEquipmentWishListFlag,
} from "@/helpers/api/apiEquipment";

/**
 * Icon and associated action to move an item of equipment between the main
 * registry and wish list
 * @param {*} equipment
 * @param {*} isWishList
 * @param {*} logout
 * @param {*} setEquipment
 * @returns
 */
const EquipmentWishListActionIcon = ({
  equipment,
  isWishList,
  logout,
  setEquipment,
}) => {
  // Set the icon depending on the direction in which the equipment will move
  const icon = isWishList ? faRecordVinyl : faHeartCirclePlus;

  /* Callback to move an album between the wish list and catalogue */
  const setEquipmentWishListFlag = useCallback(async () => {
    // Move the album to the wish list
    const result = await apiSetEquipmentWishListFlag(
      equipment,
      !isWishList,
      logout
    );
    if (result) {
      // Successful, so refresh the album list
      const fetchedEquipment = await apiFetchEquipment(isWishList, logout);
      setEquipment(fetchedEquipment);
    }
  }, [equipment, isWishList, logout, setEquipment]);

  return <FontAwesomeIcon icon={icon} onClick={setEquipmentWishListFlag} />;
};

export default EquipmentWishListActionIcon;
