import { useState, useEffect } from "react";
import { apiFetchClosestArtists } from "@/helpers/api/apiArtists";

/**
 * Hook that uses the API helpers to retrieve a list of artists closest to a target artist from the
 * Music Catalogue REST API
 * @param {*} artistId
 * @param {*} topN
 * @param {*} energy
 * @param {*} intimacy
 * @param {*} warmth
 * @param {*} mood
 * @param {*} logout
 * @returns
 */
const useClosestArtists = (artistId, topN, energy, intimacy, warmth, mood, logout) => {
  // Current list of artists and the method to change it
  const [closestArtists, setClosestArtists] = useState([]);

  useEffect(() => {
    const fetchClosestArtists = async () => {
      try {
        // Get a list of artists via the service and store it in state
        var fetchedClosestArtists = await apiFetchClosestArtists(
          artistId,
          topN,
          energy,
          intimacy,
          warmth,
          mood,
          logout
        );
        setClosestArtists(fetchedClosestArtists);
      } catch {}
    };

    fetchClosestArtists();
  }, [artistId, topN, energy, intimacy, warmth, mood, logout]);

  return { closestArtists, setClosestArtists };
};

export default useClosestArtists;
