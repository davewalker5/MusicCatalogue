import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Fetch a list of moods from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const apiFetchMoods = async (logout) => {
  // Call the API to get a list of moods
  const url = `${config.api.baseUrl}/moods`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders()
  });

  const moods = await apiReadResponseData(response, logout);
  return moods;
};

/**
 * Create a new mood
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiCreateMood = async (name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    name: name,
  });

  // Call the API to create the mood
  const url = `${config.api.baseUrl}/moods`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const mood = await apiReadResponseData(response, logout);
  return mood;
};

/**
 * Update an existing mood
 * @param {*} moodId
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiUpdateMood = async (moodId, name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: moodId,
    name: name,
  });

  // Call the API to update the mood
  const url = `${config.api.baseUrl}/moods`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const mood = await apiReadResponseData(response, logout);
  return mood;
};

/**
 * Delete an existing mood
 * @param {*} moodId
 * @param {*} logout
 * @returns
 */
const apiDeleteMood = async (moodId, logout) => {
  // Call the API to delete the mood
  const url = `${config.api.baseUrl}/moods/${moodId}`;
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

export { apiFetchMoods, apiCreateMood, apiUpdateMood, apiDeleteMood };
