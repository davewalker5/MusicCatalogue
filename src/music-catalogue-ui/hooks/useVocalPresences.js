import { useState, useEffect } from "react";
import { apiFetchVocalPresences } from "@/helpers/api/apiEnumerations";

/**
 * Hook that uses the API helpers to retrieve a list of vocal presence options from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useVocalPresences = (logout) => {
  // Current list of options and the method to change it
  const [vocalPresences, setVocalPresences] = useState([]);

  useEffect(() => {
    const fetchVocalPresences = async () => {
      try {
        // Get a list of vocal presence options via the service and store it in state
        var fetchedVocalPresences = await apiFetchVocalPresences(logout);
        setVocalPresences(fetchedVocalPresences);
      } catch {}
    };

    fetchVocalPresences();
  }, [logout]);

  return { vocalPresences, setVocalPresences };
};

export default useVocalPresences;
