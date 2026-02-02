import { useState, useEffect } from "react";
import { apiFetchTimesOfDay } from "@/helpers/api/apiEnumerations";

/**
 * Hook that uses the API helpers to retrieve a list of "times of day" options from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useTimesOfDay = (logout) => {
  // Current list of options and the method to change it
  const [timesOfDay, setTimesOfDay] = useState([]);

  useEffect(() => {
    const fetchOptions = async () => {
      try {
        // Get a list of options via the service and store it in state
        var fetchedTimesOfDay = await apiFetchTimesOfDay(null, logout);
        setTimesOfDay(fetchedTimesOfDay);
      } catch {}
    };

    fetchOptions();
  }, [logout]);

  return { timesOfDay, setTimesOfDay };
};

export default useTimesOfDay;
