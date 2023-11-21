import { apiFetchSecret, apiGetSecret } from "@/helpers/apiSecrets";
import { useState, useEffect } from "react";
import secrets from "@/helpers/secrets";

/**
 * Hook that uses the API helpers to retrieve the maps API key from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useMapsApiKey = (logout) => {
  // Current list of artists and the method to change it
  const [apiKey, setApiKey] = useState(null);

  useEffect(() => {
    const fetchApiKey = async () => {
      try {
        // Get a list of artists via the service and store it in state
        var fetchedApiKey = await apiFetchSecret(secrets.mapsApiKey);
        setApiKey(fetchedApiKey);
        apiSetSecret(fetchedApiKey);
      } catch {}
    };

    const currentApiKey = apiGetSecret(secrets.mapsApiKey);
    if (currentApiKey == null) {
      fetchApiKey();
    }
  }, [logout]);

  return { apiKey, setApiKey };
};

export default useMapsApiKey;
