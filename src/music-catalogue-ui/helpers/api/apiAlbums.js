import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * POST a request to the API to create a new album
 * @param {*} artistId
 * @param {*} genreId
 * @param {*} title
 * @param {*} released
 * @param {*} coverUrl
 * @param {*} isWishListItem
 * @param {*} purchased
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiCreateAlbum = async (
  artistId,
  genreId,
  title,
  released,
  coverUrl,
  isWishListItem,
  purchased,
  price,
  retailerId,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    artistId: artistId,
    genreId: genreId,
    title: title,
    released: released,
    coverUrl: coverUrl,
    isWishListItem: isWishListItem,
    purchased: purchased,
    price: price,
    retailerId: retailerId,
  });

  // Call the API to create the album
  const url = `${config.api.baseUrl}/albums`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const album = await apiReadResponseData(response, logout);
  return album;
};

/**
 * PUT a request to the API to update the specified album's details
 * @param {*} albumId
 * @param {*} artistId
 * @param {*} genreId
 * @param {*} title
 * @param {*} released
 * @param {*} coverUrl
 * @param {*} isWishListItem
 * @param {*} purchased
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiUpdateAlbum = async (
  albumId,
  artistId,
  genreId,
  title,
  released,
  coverUrl,
  isWishListItem,
  purchased,
  price,
  retailerId,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    id: albumId,
    artistId: artistId,
    genreId: genreId,
    title: title,
    released: released,
    coverUrl: coverUrl,
    isWishListItem: isWishListItem,
    purchased: purchased,
    price: price,
    retailerId: retailerId,
  });

  // Call the API to update the album
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
  // Send the update request to the API and return the response
  const response = await apiUpdateAlbum(
    album.id,
    album.artistId,
    album.genreId,
    album.title,
    album.released,
    album.coverUrl,
    wishListFlag,
    album.purchased,
    album.price,
    album.retailerId,
    logout
  );
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
  // Send the update request to the API and return the response
  const response = await apiUpdateAlbum(
    album.id,
    album.artistId,
    album.genreId,
    album.title,
    album.released,
    album.coverUrl,
    album.isWishListItem,
    purchaseDate,
    price,
    retailerId,
    logout
  );
  return response;
};

export {
  apiCreateAlbum,
  apiUpdateAlbum,
  apiDeleteAlbum,
  apiFetchAlbumsByArtist,
  apiFetchAlbumById,
  apiLookupAlbum,
  apiSetAlbumWishListFlag,
  apiSetAlbumPurchaseDetails
};
