import { useState, useEffect } from "react";
import { apiFetchEnsembleTypes } from "@/helpers/api/apiEnumerations";

/**
 * Hook that uses the API helpers to retrieve a list of ensemble type options from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useEnsembleTypes = (logout) => {
  // Current list of options and the method to change it
  const [ensembleTypes, setEnsembleTypes] = useState([]);

  useEffect(() => {
    const fetchOptions = async () => {
      try {
        // Get a list of options via the service and store it in state
        var fetchedEnsembleTypes = await apiFetchEnsembleTypes(null, logout);
        setEnsembleTypes(fetchedEnsembleTypes);
      } catch {}
    };

    fetchOptions();
  }, [logout]);

  return { ensembleTypes, setEnsembleTypes };
};

export default useEnsembleTypes;
