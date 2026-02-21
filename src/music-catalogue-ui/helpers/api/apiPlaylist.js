import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders } from "./apiHeaders";

/**
 * POST a request to the API to generate a playlist
 * @param {*} playlistTypeId
 * @param {*} timeOfDayId
 * @param {*} numberOfEntries
 * @param {*} currentArtistId
 * @param {*} includedGenreIds
 * @param {*} excludedGenreIds
 * @param {*} logout
 * @returns
 */
const apiGeneratePlaylist = async (
  playlistTypeId,
  timeOfDayId,
  numberOfEntries,
  currentArtistId,
  includedGenreIds,
  excludedGenreIds,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    type: playlistTypeId,
    timeOfDay: timeOfDayId,
    currentArtistId: currentArtistId,
    numberOfEntries: numberOfEntries,
    includedGenreIds: includedGenreIds,
    excludedGenreIds: excludedGenreIds
  });

  // Call the API to create the album
  const url = `${config.api.baseUrl}/playlist/generate`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const playlist = await apiReadResponseData(response, logout);
  return playlist;
};

/**
 * POST a request to the API to save a playlist as a saved session
 * @param {*} playlistTypeId 
 * @param {*} timeOfDayId 
 * @param {*} albumIds
 * @param {*} logout 
 * @returns 
 */
const apiSavePlaylist = async (
  playlistTypeId,
  timeOfDayId,
  albumIds,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    type: playlistTypeId,
    timeOfDay: timeOfDayId,
    albumIds: albumIds
  });

  // Call the API to create the album
  const url = `${config.api.baseUrl}/playlist/save`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const session = await apiReadResponseData(response, logout);
  return session;
};

/**
 * POST a request to the API to load saved sessions matching specified criteria
 * @param {*} fromDate 
 * @param {*} toDate 
 * @param {*} playlistTypeId 
 * @param {*} timeOfDayId
 * @param {*} logout 
 * @returns 
 */
const apiSearchForSessions = async (
  fromDate,
  toDate,
  playlistTypeId,
  timeOfDayId,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    from: fromDate,
    to: toDate,
    type: playlistTypeId,
    timeOfDay: timeOfDayId
  });

  // Call the API to search for matching sessions
  const url = `${config.api.baseUrl}/playlist/search`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const sessions = await apiReadResponseData(response, logout);
  return sessions;
};

/**
 * POST a request to the API to load expoert a session
 * @param {*} sessionId 
 * @param {*} fileName
 * @param {*} logout 
 * @returns 
 */
const apiExportSession = async (
  sessionId,
  fileName,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    sessionId: sessionId,
    fileName: fileName
  });

  // Call the API to export the session
  const url = `${config.api.baseUrl}/export/session`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  }

  return response.ok;
};

export { apiGeneratePlaylist, apiSavePlaylist, apiSearchForSessions, apiExportSession };