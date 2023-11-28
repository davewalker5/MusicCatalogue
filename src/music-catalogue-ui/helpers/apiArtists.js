import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetHeaders, apiGetPostHeaders } from "./apiHeaders";

/**
 * POST a request to the API to create a new artist
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiCreateArtist = async (name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    name: name,
  });

  // Call the API to create the artist
  const url = `${config.api.baseUrl}/artists`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const artist = await apiReadResponseData(response, logout);
  return artist;
};

/**
 * PUT a request to the API to update an existing artist
 * @param {*} artistId
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiUpdateArtist = async (artistId, name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: artistId,
    name: name,
  });

  // Call the API to update the artist
  const url = `${config.api.baseUrl}/artists`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const artist = await apiReadResponseData(response, logout);
  return artist;
};

/**
 * Fetch a list of artists from the Music Catalogue REST API
 * @param {*} filter
 * @param {*} genreId
 * @param {*} isWishList
 * @param {*} withNoAlbums
 * @param {*} logout
 * @returns
 */
const apiFetchArtists = async (
  filter,
  genreId,
  isWishList,
  withNoAlbums,
  logout
) => {
  // Construct the filtering criteria as the request body and convert to JSON
  const criteria = {
    namePrefix: filter,
    wishList: isWishList,
    genreId: genreId,
    includeArtistsWithNoAlbums: withNoAlbums,
  };
  const body = JSON.stringify(criteria);

  // Call the API to get a list of all artists
  const url = `${config.api.baseUrl}/artists/search/`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
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

/**
 * Send a request to the API to delete an existing artist
 * @param {*} artistId
 * @param {*} logout
 * @returns
 */
const apiDeleteArtist = async (artistId, logout) => {
  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/artists/${artistId}`;
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

export {
  apiCreateArtist,
  apiUpdateArtist,
  apiDeleteArtist,
  apiFetchArtists,
  apiFetchArtistById,
};
