import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders } from "./apiHeaders";

/**
 * POST a request to the API to generate a playlist
 * @param {*} playlistTypeId
 * @param {*} timeOfDayId
 * @param {*} numberOfEntries
 * @param {*} logout
 * @returns
 */
const apiGeneratePlaylist = async (
  playlistTypeId,
  timeOfDayId,
  numberOfEntries,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    type: playlistTypeId,
    timeOfDay: timeOfDayId,
    numberOfEntries: numberOfEntries,
  });

  // Call the API to create the album
  const url = `${config.api.baseUrl}/search/playlist`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const album = await apiReadResponseData(response, logout);
  return album;
};

export { apiGeneratePlaylist };