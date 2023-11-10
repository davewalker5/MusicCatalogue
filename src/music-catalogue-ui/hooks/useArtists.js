import { useState, useEffect } from "react";
import { apiFetchAllArtists } from "@/helpers/api";

/**
 * Hook that uses the API helpers to retrieve a list of artists from the
 * Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const useArtists = (isWishlist, logout) => {
  // Current list of artists and the method to change it
  const [artists, setArtists] = useState([]);

  useEffect(() => {
    const fetchArtists = async () => {
      try {
        // Get a list of artists via the service, store it in state and clear the
        // loading status
        var fetchedArtists = await apiFetchAllArtists(isWishlist, logout);
        setArtists(fetchedArtists);
      } catch {}
    };

    fetchArtists();
  }, [isWishlist, logout]);

  return { artists, setArtists };
};

export default useArtists;
