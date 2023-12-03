import config from "@/config.json";
import { apiGetHeaders } from "./apiHeaders";

/**
 * Fetch a secret from the Music Catalogue REST API
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiFetchSecret = async (name, logout) => {
  // URL encode the lookup properties
  const encodedName = encodeURIComponent(name);

  // Call the API to get the specified secret
  const url = `${config.api.baseUrl}/secrets/${encodedName}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else if (response.status == 204) {
    // If the response is "no content", return NULL
    return null;
  } else if (response.ok) {
    // Get the response content as JSON and return it
    const secret = await response.text();
    return secret;
  } else {
    return null;
  }
};

export { apiFetchSecret };
