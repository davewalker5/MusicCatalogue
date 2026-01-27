import { useState, useEffect } from "react";
import { apiFetchGenres } from "@/helpers/api/apiGenres";

/**
 * Hook that uses the API helpers to retrieve a list of genres from the
 * Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const useGenres = (logout) => {
  // Current list of artists and the method to change it
  const [genres, setGenres] = useState([]);

  useEffect(() => {
    const fetchGenres = async () => {
      try {
        // Get a list of genres via the service and store it in state
        var fetchedGenres = await apiFetchGenres(null, logout);
        setGenres(fetchedGenres);
      } catch {}
    };

    fetchGenres();
  }, [logout]);

  return { genres, setGenres };
};

export default useGenres;
