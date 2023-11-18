import { useState, useEffect } from "react";
import { apiFetchArtists } from "@/helpers/apiArtists";

/**
 * Hook that uses the API helpers to retrieve a list of artists from the
 * Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const useArtists = (filter, genre, isWishlist, logout) => {
  // Current list of artists and the method to change it
  const [artists, setArtists] = useState([]);

  useEffect(() => {
    const fetchArtists = async () => {
      try {
        // Get the genre Id
        const genreId = genre != null ? genre.id : null;

        // Get a list of artists via the service and store it in state
        var fetchedArtists = await apiFetchArtists(
          filter,
          genreId,
          isWishlist,
          logout
        );
        setArtists(fetchedArtists);
      } catch {}
    };

    fetchArtists();
  }, [filter, genre, isWishlist, logout]);

  return { artists, setArtists };
};

export default useArtists;
