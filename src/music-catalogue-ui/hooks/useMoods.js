import { useState, useEffect } from "react";
import { apiFetchMoods } from "@/helpers/api/apiMoods";

/**
 * Hook that uses the API helpers to retrieve a list of moods from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useMoods = (logout) => {
  // Current list of moods and the method to change it
  const [moods, setMoods] = useState([]);

  useEffect(() => {
    const fetchMoods = async () => {
      try {
        // Get a list of moods via the service and store it in state
        var fetchedMoods = await apiFetchMoods(logout);
        setMoods(fetchedMoods);
      } catch {}
    };

    fetchMoods();
  }, [logout]);

  return { moods, setMoods };
};

export default useMoods;
