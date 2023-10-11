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
  // TODO: Move to HTTP Cookie
  return localStorage.getItem("token");
};

/**
 * Clear the current JWT token
 */
const apiClearToken = () => {
  // TODO: Move to HTTP Cookie
  localStorage.removeItem("token");
};

/**
 * Return the HTTP headers used when calling the REST API
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
 * @param {*} logout
 * @returns
 */
const apiFetchAllArtists = async (logout) => {
  // Call the API to get a list of all artists
  const url = `${config.api.baseUrl}/artists/`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const artists = await response.json();
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
    const artist = await response.json();
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
 * @param {*} logout
 * @returns
 */
const apiFetchAlbumsByArtist = async (artistId, logout) => {
  // Call the API to get a list of all albums by the specified artist
  const url = `${config.api.baseUrl}/albums/artist/${artistId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const albums = await response.json();
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
  // Call the API to get the details for the specifiedf album
  const url = `${config.api.baseUrl}/albums/${albumId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await response.json();
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
};

/**
 * Look up an album using the REST API, calling the external service for the
 * details if not found in the local database
 * @param {*} artistName
 * @param {*} albumTitle
 * @param {*} logout
 */
const apiLookupAlbum = async (artistName, albumTitle, logout) => {
  // URL encode the lookup properties
  const encodedArtistName = encodeURIComponent(artistName);
  const encodedAlbumTitle = encodeURIComponent(albumTitle);

  // Call the API to get the details for the specifiedf album
  const url = `${config.api.baseUrl}/search/${encodedArtistName}/${encodedAlbumTitle}`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await response.json();
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    return null;
  }
};

export {
  apiSetToken,
  apiClearToken,
  apiAuthenticate,
  apiFetchAllArtists,
  apiFetchArtistById,
  apiFetchAlbumsByArtist,
  apiFetchAlbumById,
  apiLookupAlbum,
};
