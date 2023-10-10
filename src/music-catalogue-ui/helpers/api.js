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
const apiGetToken = (token) => {
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
  // Get the token
  var token = apiGetToken();

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  // Call the API to get a list of all artists
  const url = `${config.api.baseUrl}/artists/`;
  const response = await fetch(url, {
    method: "GET",
    headers: headers,
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const artists = await response.json();
    return artists;
  } else {
    // Unauthorized so the token's likely expired - force a login
    logout();
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
  // Get the token
  var token = apiGetToken();

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  // Call the API to get a list of all albums by the specified artist
  const url = `${config.api.baseUrl}/albums/artist/${artistId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: headers,
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const albums = await response.json();
    return albums;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
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
  // Get the token
  var token = apiGetToken();

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  // Call the API to get the details for the specifiedf album
  const url = `${config.api.baseUrl}/albums/${albumId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: headers,
  });

  if (response.ok) {
    // Get the response content as JSON and return it
    const album = await response.json();
    return album;
  } else if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  }
};

export {
  apiAuthenticate,
  apiFetchAllArtists,
  apiFetchAlbumsByArtist,
  apiFetchAlbumById,
  apiSetToken,
  apiClearToken,
};
