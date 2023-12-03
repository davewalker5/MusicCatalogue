import { useState, useEffect } from "react";
import { apiFetchAlbumsByArtist } from "@/helpers/api/apiAlbums";

/**
 * Hook that uses the API helpers to retrieve a list of albums by the specified
 * artist from the Music Catalogue REST API
 * @param {*} artistId
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const useAlbums = (artistId, isWishList, logout) => {
  // Current list of albums and the method to change it
  const [albums, setAlbums] = useState([]);

  useEffect(() => {
    const fetchAlbums = async (artistId) => {
      try {
        // Get a list of albums via the service and store it in state
        var fetchedAlbums = await apiFetchAlbumsByArtist(
          artistId,
          isWishList,
          logout
        );
        setAlbums(fetchedAlbums);
      } catch {}
    };

    fetchAlbums(artistId);
  }, [artistId, isWishList, logout]);

  return { albums, setAlbums };
};

export default useAlbums;
