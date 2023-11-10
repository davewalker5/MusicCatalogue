import config from "../config.json";

/**
 * Store the JWT token
 * @param {*} token
 */
const apiSetToken = (token) => {
  // TODO: Move to HTTP Cookie
  localStorage.setItem("token", token);
};

/**
 * Retrieve the current JWT token
 * @param {*} token
 * @returns
 */
const apiGetToken = () => {
  try {
    // TODO: Move to HTTP Cookie
    const token = localStorage.getItem("token");
    return token;
  } catch {
    return null;
  }
};

/**
 * Clear the current JWT token
 */
const apiClearToken = () => {
  // TODO: Move to HTTP Cookie
  localStorage.removeItem("token");
};

/**
 * Return the HTTP headers used when sending GET requests to the REST API
 * @returns
 */
const apiGetHeaders = () => {
  // Get the token
  var token = apiGetToken();

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  return headers;
};

/**
 * Return the HTTP headers used when sending POST and PUT requests to the REST API
 * @returns
 */
const apiGetPostHeaders = () => {
  // Get the token
  var token = apiGetToken();

  // Construct the request headers
  const headers = {
    Accept: "application/json",
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`,
  };

  return headers;
};

/**
 * Format a date/time in a manner suitable for use with the API without using
 * 3rd party libraries
 * @param {F} date
 */
const apiFormatDateTime = (date) => {
  // Get the formatted date components
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");

  // Get the formatted time components
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");
  const seconds = date.getSeconds().toString().padStart(2, "0");

  // Construct and return the formatted date/time
  const formattedDate =
    year +
    "-" +
    month +
    "-" +
    day +
    "%20" +
    hours +
    ":" +
    minutes +
    ":" +
    seconds;
  return formattedDate;
};

/**
 * Read the response content as JSON and return the resulting object
 * @param {*} response
 * @returns
 */
const apiReadResponseData = async (response) => {
  // The API can return a No Content response so check for that first
  if (response.status == 204) {
    // No content response, so return null
    return null;
  } else {
    // Get the response content as JSON and return it
    const data = await response.json();
    return data;
  }
};

/**
 * Authenticate with the Music Catalogue REST API
 * @param {*} username
 * @param {*} password
 * @returns
 */
const apiAuthenticate = async (username, password) => {
  // Create a JSON body containing the credentials
  const body = JSON.stringify({
    userName: username,
    password: password,
  });

  // Call the API to authenticate as the specified user and return a token
  const url = `${config.api.baseUrl}/users/authenticate/`;
  const response = await fetch(url, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: body,
  });

  // Get the response text
  const token = await response.text();
  return token.replace(/"/g, "");
};

/**
 * Fetch a list of all artists from the Music Catalogue REST API
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const apiFetchAllArtists = async (isWishList, logout) => {
  // Call the API to get a list of all artists
  const url = `${config.api.baseUrl}/artists/${isWishList}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const artists = await apiReadResponseData(response);
    return artists;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
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

  if (response.ok) {
    // Get the response content as JSON and return it
    const artist = await apiReadResponseData(response);
    return artist;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
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
  // Call the API to get a list of all albums by the specified artist
  const url = `${config.api.baseUrl}/albums/artist/${artistId}/${isWishList}`;
  console.log(url);
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const albums = await apiReadResponseData(response);
    return albums;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
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

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await apiReadResponseData(response);
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
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

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await apiReadResponseData(response);
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
};

/**
 * Set the wish list flag on an album
 * @param {*} album
 * @param {*} wishListFlag
 * @param {*} logout
 * @returns
 */
const apiSetAlbumWishListFlag = async (album, wishListFlag, logout) => {
  // Construct the body - the wish list flat needs to be updated before this
  // and there's no need to send the track information - an empty array will do
  album.isWishListItem = wishListFlag;
  album.tracks = [];
  const body = JSON.stringify(album);
  console.log(body);

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/albums`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await apiReadResponseData(response);
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
};

/**
 * Request an export of the catalogue
 * @param {*} fileName
 * @param {*} logout
 */
const apiRequestExport = async (fileName, logout) => {
  // Create a JSON body containing the file name to export to
  const body = JSON.stringify({
    fileName: fileName,
  });

  // Call the API to request the export
  const url = `${config.api.baseUrl}/export/catalogue`;
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

const apiJobStatusReport = async (from, to, logout) => {
  // Make sure the dates cover the period 00:00:00 on the start date to 23:59:59 on the
  // end date
  const startDate = new Date(
    from.getFullYear(),
    from.getMonth(),
    from.getDate(),
    0,
    0,
    0
  );

  const endDate = new Date(
    to.getFullYear(),
    to.getMonth(),
    to.getDate(),
    23,
    59,
    59
  );

  // Construct the route. Dates need to be formatted in a specific fashion for the API
  // to decode them and they also need to be URL encoded
  const fromRouteSegment = apiFormatDateTime(startDate);
  const toRouteSegment = apiFormatDateTime(endDate);
  const url = `${config.api.baseUrl}/reports/jobs/${fromRouteSegment}/${toRouteSegment}`;
  console.log(url);
  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const records = await apiReadResponseData(response);
    return records;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
};

export {
  apiSetToken,
  apiGetToken,
  apiClearToken,
  apiAuthenticate,
  apiFetchAllArtists,
  apiFetchArtistById,
  apiFetchAlbumsByArtist,
  apiFetchAlbumById,
  apiDeleteAlbum,
  apiLookupAlbum,
  apiSetAlbumWishListFlag,
  apiRequestExport,
  apiJobStatusReport,
};
