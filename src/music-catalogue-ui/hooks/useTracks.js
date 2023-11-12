import { useState, useEffect } from "react";
import { apiFetchAlbumById } from "@/helpers/apiAlbums";

/**
 * Hook that uses the API helpers to retrieve a list of tracks for the
 * specified album from the Music Catalogue REST API
 * @param {*} albumId
 * @param {*} logout
 * @returns
 */
const useTracks = (albumId, logout) => {
  // Current list of tracks and the method to change it
  const [tracks, setTracks] = useState([]);

  useEffect(() => {
    const fetchTracks = async (albumId) => {
      try {
        // Get the album from the service and set the tracks to the appropriate
        // member of the returned object
        var fetchedAlbum = await apiFetchAlbumById(albumId, logout);
        setTracks(fetchedAlbum.tracks);
      } catch {}
    };

    fetchTracks(albumId);
  }, [albumId, logout]);

  return { tracks, setTracks };
};

export default useTracks;
