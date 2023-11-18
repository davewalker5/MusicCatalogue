import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders } from "./apiHeaders";

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

export { apiFetchGenres };
