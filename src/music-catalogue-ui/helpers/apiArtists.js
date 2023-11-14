import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetHeaders } from "./apiHeaders";

/**
 * Fetch a list of all artists from the Music Catalogue REST API
 * @param {*} filter
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const apiFetchArtists = async (filter, isWishList, logout) => {
  // Call the API to get a list of all artists
  const url = `${config.api.baseUrl}/artists/${filter}/${isWishList}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const artists = await apiReadResponseData(response, logout);
  return artists;
};

/**
 * Fetch the details for a single artist from the Music Catalogue REST API
 * given the artist ID
 * @param {*} artistId
 * @param {*} logout
 * @returns
 */
const apiFetchArtistById = async (artistId, logout) => {
  // Call the API to get the artist details
  const url = `${config.api.baseUrl}/artists/${artistId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const artist = await apiReadResponseData(response, logout);
  return artist;
};

export { apiFetchArtists, apiFetchArtistById };
