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
 * @param {*} fileName
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
  fileName,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    type: playlistTypeId,
    timeOfDay: timeOfDayId,
    currentArtistId: currentArtistId,
    numberOfEntries: numberOfEntries,
    includedGenreIds: includedGenreIds,
    excludedGenreIds: excludedGenreIds,
    fileName: fileName
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

export { apiGeneratePlaylist, apiSavePlaylist };