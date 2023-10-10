import statuses from "@/helpers/status";
import { useState, useEffect } from "react";
import { apiFetchAlbumsByArtist } from "@/helpers/api";

const useAlbums = (artistId, logout) => {
  // Current list of albums and the method to change it
  const [albums, setAlbums] = useState([]);

  // Current status indicator and the method to change it, defaults to "loading"
  const [currentStatus, setCurrentStatus] = useState(statuses.isLoading);

  useEffect(() => {
    const fetchAlbums = async (artistId) => {
      // Set the "loading" indicator
      setCurrentStatus(statuses.isLoading);

      try {
        // Get a list of albums via the service, store it in state and clear the
        // loading status
        var fetchedAlbums = await apiFetchAlbumsByArtist(artistId, logout);
        setAlbums(fetchedAlbums);
        setCurrentStatus(statuses.loaded);
      } catch {
        setCurrentStatus(statuses.hasErrored);
      }
    };

    fetchAlbums(artistId);
  }, []);

  return { albums, setAlbums, currentStatus };
};

export default useAlbums;
