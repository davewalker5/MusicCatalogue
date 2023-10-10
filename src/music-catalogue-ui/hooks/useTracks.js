import statuses from "@/helpers/status";
import { useState, useEffect } from "react";
import { apiFetchAlbumById } from "@/helpers/api";

const useTracks = (albumId, logout) => {
  // Current list of tracks and the method to change it
  const [tracks, setTracks] = useState([]);

  // Current status indicator and the method to change it, defaults to "loading"
  const [currentStatus, setCurrentStatus] = useState(statuses.isLoading);

  useEffect(() => {
    const fetchTracks = async (albumId) => {
      // Set the "loading" indicator
      setCurrentStatus(statuses.isLoading);

      try {
        // Get the album from the service and set the tracks to the appropriate
        // member of the returned object
        var fetchedAlbum = await apiFetchAlbumById(albumId, logout);
        setTracks(fetchedAlbum.tracks);
        setCurrentStatus(statuses.loaded);
      } catch {
        setCurrentStatus(statuses.hasErrored);
      }
    };

    fetchTracks(albumId);
  }, []);

  return { tracks, setTracks, currentStatus };
};

export default useTracks;
