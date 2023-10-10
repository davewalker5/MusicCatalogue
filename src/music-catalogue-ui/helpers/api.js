import config from "../config.json";

const apiSetToken = (token) => {
  // TODO: Move to HTTP Cookie
  localStorage.setItem("token", token);
};

const apiGetToken = (token) => {
  // TODO: Move to HTTP Cookie
  return localStorage.getItem("token");
};

const apiClearToken = () => {
  // TODO: Move to HTTP Cookie
  localStorage.removeItem("token");
};

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
