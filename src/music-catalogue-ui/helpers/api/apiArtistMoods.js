import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create a new artist/mood mapping
 * @param {*} artistId
 * @param {*} moodId
 * @param {*} logout
 * @returns
 */
const apiCreateArtistMood = async (artistId, moodId, logout) => {
  // Construct the body
  const body = JSON.stringify({
    artistId: artistId,
    moodId: moodId
  });

  // Call the API to create the mapping
  const url = `${config.api.baseUrl}/artistmoods`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const mood = await apiReadResponseData(response, logout);
  return mood;
};

/**
 * Delete an existing artist/mood mapping
 * @param {*} id
 * @param {*} logout
 * @returns
 */
const apiDeleteArtistMood = async (id, logout) => {
  // Call the API to delete the mood
  const url = `${config.api.baseUrl}/artistmoods/${id}`;
  const response = await fetch(url, {
    method: "DELETE",
    headers: apiGetHeaders(),
  });

  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    // Return the response status code
    return response.ok;
  }
};

export { apiCreateArtistMood, apiDeleteArtistMood };
