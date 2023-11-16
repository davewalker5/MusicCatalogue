import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * POST a request to the API to update the specified album's details
 * @param {*} album
 * @param {*} logout
 * @returns
 */
const apiAlbumUpdate = async (album, logout) => {
  // Construct the body - the wish list flat needs to be updated before this
  const body = JSON.stringify(album);

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/albums`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const updatedAlbum = await apiReadResponseData(response, logout);
  return updatedAlbum;
};

/**
 * Fetch a list of albums by the specified artist from the Music Catalogue
 * REST API
 * @param {*} artistId
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const apiFetchAlbumsByArtist = async (artistId, isWishList, logout) => {
  // Construct the filtering criteria as the request body and convert to JSON
  const criteria = {
    artistId: artistId,
    wishList: isWishList,
    genreId: null,
  };
  const body = JSON.stringify(criteria);

  // Call the API to get a list of all albums by the specified artist
  const url = `${config.api.baseUrl}/albums/search/`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const albums = await apiReadResponseData(response, logout);
  return albums;
};

/**
 * Return the album details and track list for the specified album from the
 * Music Catalogue REST API
 * @param {*} albumId
 * @param {*} logout
 * @returns
 */
const apiFetchAlbumById = async (albumId, logout) => {
  // Call the API to get the details for the specified album
  const url = `${config.api.baseUrl}/albums/${albumId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const album = await apiReadResponseData(response, logout);
  return album;
};

/**
 * Delete the album with the specified ID, along with all its tracks
 * @param {*} albumId
 * @param {*} logout
 * @returns
 */
const apiDeleteAlbum = async (albumId, logout) => {
  // Call the API to delete the specified album
  const url = `${config.api.baseUrl}/albums/${albumId}`;
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

/**
 * Look up an album using the REST API, calling the external service for the
 * details if not found in the local database
 * @param {*} artistName
 * @param {*} albumTitle
 * @param {*} storeInWishList
 * @param {*} logout
 */
const apiLookupAlbum = async (
  artistName,
  albumTitle,
  storeInWishList,
  logout
) => {
  // URL encode the lookup properties
  const encodedArtistName = encodeURIComponent(artistName);
  const encodedAlbumTitle = encodeURIComponent(albumTitle);

  // Call the API to get the details for the specified album
  const url = `${config.api.baseUrl}/search/${encodedArtistName}/${encodedAlbumTitle}/${storeInWishList}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const album = await apiReadResponseData(response, logout);
  return album;
};

/**
 * Set the wish list flag on an album
 * @param {*} album
 * @param {*} wishListFlag
 * @param {*} logout
 * @returns
 */
const apiSetAlbumWishListFlag = async (album, wishListFlag, logout) => {
  // Update the album properties
  album.isWishListItem = wishListFlag;

  // Send the update request to the API and return the response
  const response = await apiAlbumUpdate(album, logout);
  return response;
};

/**
 * Set the purchase details for the specified album
 * @param {*} album
 * @param {*} purchaseDate
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiSetAlbumPurchaseDetails = async (
  album,
  purchaseDate,
  price,
  retailerId,
  logout
) => {
  // Update the purchase details
  album.purchased = purchaseDate;
  album.price = price;
  album.retailerId = retailerId;

  // Send the update request to the API and return the response
  const response = await apiAlbumUpdate(album, logout);
  return response;
};

export {
  apiFetchAlbumsByArtist,
  apiFetchAlbumById,
  apiDeleteAlbum,
  apiLookupAlbum,
  apiSetAlbumWishListFlag,
  apiSetAlbumPurchaseDetails,
};
