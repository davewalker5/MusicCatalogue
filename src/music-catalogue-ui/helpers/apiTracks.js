import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create a track
 * @param {*} title
 * @param {*} number
 * @param {*} duration
 * @param {*} albumId
 * @param {*} logout
 * @returns
 */
const apiCreateTrack = async (title, number, duration, albumId, logout) => {
  // Create the request body
  const body = JSON.stringify({
    title: title,
    number: number,
    duration: duration,
    albumId: albumId,
  });

  // Call the API to create the retailer. This will just return the current
  // record if it already exists
  const url = `${config.api.baseUrl}/tracks`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const track = await apiReadResponseData(response, logout);
  return track;
};

/**
 * Update track details
 * @param {*} id
 * @param {*} title
 * @param {*} number
 * @param {*} duration
 * @param {*} albumId
 * @param {*} logout
 * @returns
 */
const apiUpdateTrack = async (id, title, number, duration, albumId, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: id,
    title: title,
    number: number,
    duration: duration,
    albumId: albumId,
  });

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/tracks`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const updatedTrack = await apiReadResponseData(response, logout);
  return updatedTrack;
};

/**
 * Delete the track with the specified ID
 * @param {*} trackId
 * @param {*} logout
 * @returns
 */
const apiDeleteTrack = async (trackId, logout) => {
  // Call the API to delete the specified album
  const url = `${config.api.baseUrl}/tracks/${trackId}`;
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

export { apiCreateTrack, apiUpdateTrack, apiDeleteTrack };
