import { useState, useEffect } from "react";
import { apiFetchAlbumsByArtist } from "@/helpers/api";

/**
 * Hook that uses the API helpers to retrieve a list of albums by the specified
 * artist from the Music Catalogue REST API
 * @param {*} artistId
 * @param {*} logout
 * @returns
 */
const useAlbums = (artistId, logout) => {
  // Current list of albums and the method to change it
  const [albums, setAlbums] = useState([]);

  useEffect(() => {
    const fetchAlbums = async (artistId) => {
      try {
        // Get a list of albums via the service, store it in state and clear the
        // loading status
        var fetchedAlbums = await apiFetchAlbumsByArtist(artistId, logout);
        setAlbums(fetchedAlbums);
      } catch {}
    };

    fetchAlbums(artistId);
  }, [artistId, logout]);

  return { albums, setAlbums };
};

export default useAlbums;
