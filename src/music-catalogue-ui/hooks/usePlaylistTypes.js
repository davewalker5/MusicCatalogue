import { useState, useEffect } from "react";
import { apiFetchPlaylistTypes } from "@/helpers/api/apiEnumerations";

/**
 * Hook that uses the API helpers to retrieve a list of playlist type options from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const usePlaylistTypes = (logout) => {
  // Current list of options and the method to change it
  const [playlistTypes, setPlaylistTypes] = useState([]);

  useEffect(() => {
    const fetchOptions = async () => {
      try {
        // Get a list of options via the service and store it in state
        var fetchedPlaylistTypes = await apiFetchPlaylistTypes(null, logout);
        setPlaylistTypes(fetchedPlaylistTypes);
      } catch {}
    };

    fetchOptions();
  }, [logout]);

  return { playlistTypes, setPlaylistTypes };
};

export default usePlaylistTypes;
