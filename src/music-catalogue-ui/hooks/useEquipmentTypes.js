import { useState, useEffect } from "react";
import { apiFetchEquipmentTypes } from "@/helpers/api/apiEquipmentTypes";

/**
 * Hook that uses the API helpers to retrieve a list of equipment types
 * from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useEquipmentTypes = (logout) => {
  // Current list of equipment types and the method to change it
  const [equipmentTypes, setEquipmentTypes] = useState([]);

  useEffect(() => {
    const fetchEquipmentTypes = async () => {
      try {
        // Get a list of equipment types via the service and store it in state
        var fetchedEquipmentTypes = await apiFetchEquipmentTypes(logout);
        setEquipmentTypes(fetchedEquipmentTypes);
      } catch {}
    };

    fetchEquipmentTypes();
  }, [logout]);

  return { equipmentTypes, setEquipmentTypes };
};

export default useEquipmentTypes;
