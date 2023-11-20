import { apiFetchRetailers } from "@/helpers/apiRetailers";
import { useState, useEffect } from "react";

/**
 * Hook that uses the API helpers to retrieve a list of retailers from the
 * Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const useRetailers = (logout) => {
  // Current list of retailers and the method to change it
  const [retailers, setRetailers] = useState([]);

  useEffect(() => {
    const fetchReetailers = async () => {
      try {
        // Get a list of retailers via the service and store it in state
        var fetchedRetailers = await apiFetchRetailers(logout);
        setRetailers(fetchedRetailers);
      } catch {}
    };

    fetchReetailers();
  }, [logout]);

  return { retailers: retailers, setRetailers: setRetailers };
};

export default useRetailers;
