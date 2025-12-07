import { useState, useEffect } from "react";
import { apiFetchManufacturers } from "@/helpers/api/apiManufacturers";

/**
 * Hook that uses the API helpers to retrieve a list of manufacturers
 * from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useManufacturers = (logout) => {
  // Current list of manufacturers and the method to change it
  const [manufacturers, setManufacturers] = useState([]);

  useEffect(() => {
    const fetchEquipmentTypes = async () => {
      try {
        // Get a list of manufacturers via the service and store it in state
        var fetchedManufacturers = await apiFetchManufacturers(logout);
        setManufacturers(fetchedManufacturers);
      } catch {}
    };

    fetchEquipmentTypes();
  }, [logout]);

  return { manufacturers, setManufacturers };
};

export default useManufacturers;
