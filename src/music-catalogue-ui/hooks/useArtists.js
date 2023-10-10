import statuses from "@/helpers/status";
import { useState, useEffect } from "react";
import { apiFetchAllArtists } from "@/helpers/api";

const useArtists = (logout) => {
  // Current list of artists and the method to change it
  const [artists, setArtists] = useState([]);

  // Current status indicator and the method to change it, defaults to "loading"
  const [currentStatus, setCurrentStatus] = useState(statuses.isLoading);

  useEffect(() => {
    const fetchArtists = async () => {
      // Set the "loading" indicator
      setCurrentStatus(statuses.isLoading);

      try {
        // Get a list of artists via the service, store it in state and clear the
        // loading status
        var fetchedArtists = await apiFetchAllArtists(logout);
        setArtists(fetchedArtists);
        setCurrentStatus(statuses.loaded);
      } catch {
        setCurrentStatus(statuses.hasErrored);
      }
    };

    fetchArtists();
  }, []);

  return { artists, setArtists, currentStatus };
};

export default useArtists;
