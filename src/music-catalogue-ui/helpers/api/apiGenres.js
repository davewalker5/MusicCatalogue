import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Fetch a list of genres from the Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const apiFetchGenres = async (isWishList, logout) => {
  // Construct the filtering criteria as the request body and convert to JSON
  const criteria = {
    wishList: isWishList,
  };
  const body = JSON.stringify(criteria);

  // Call the API to get a list of genres
  const url = `${config.api.baseUrl}/genres/search/`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const genres = await apiReadResponseData(response, logout);
  return genres;
};

/**
 * Create a new genre
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiCreateGenre = async (name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    name: name,
  });

  // Call the API to create the genre
  const url = `${config.api.baseUrl}/genres`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const genre = await apiReadResponseData(response, logout);
  return genre;
};

/**
 * Update an existing genre
 * @param {*} genreId
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiUpdateGenre = async (genreId, name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: genreId,
    name: name,
  });

  // Call the API to update the genre
  const url = `${config.api.baseUrl}/genres`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const genre = await apiReadResponseData(response, logout);
  return genre;
};

/**
 * Delete an existing genre
 * @param {*} genreId
 * @param {*} logout
 * @returns
 */
const apiDeleteGenre = async (genreId, logout) => {
  // Call the API to delete the genre
  const url = `${config.api.baseUrl}/genres/${genreId}`;
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

export { apiFetchGenres, apiCreateGenre, apiUpdateGenre, apiDeleteGenre };
