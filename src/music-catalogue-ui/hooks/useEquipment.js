import { apiFetchEquipment } from "@/helpers/api/apiEquipment";
import { useState, useEffect } from "react";

/**
 * Hook that uses the API helpers to retrieve a list of equipment from
 * the Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const useEquipment = (isWishList, logout) => {
  // Current list of albums and the method to change it
  const [equipment, setEquipment] = useState([]);

  useEffect(() => {
    const fetchEquipment = async () => {
      try {
        // Get a list of albums via the service and store it in state
        var fetchedEquipment = await apiFetchEquipment(isWishList, logout);
        setEquipment(fetchedEquipment);
      } catch {}
    };

    fetchEquipment();
  }, [isWishList, logout]);

  return { equipment, setEquipment };
};

export default useEquipment;
