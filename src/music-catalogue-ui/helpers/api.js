import { baseUrl, username, password } from "@/helpers/constants";

const apiAuthenticate = async (username, password) => {
  // Create a JSON body containing the credentials
  const body = JSON.stringify({
    userName: username,
    password: password,
  });

  // Call the API to authenticate as the specified user and return a token
  const url = baseUrl + "/users/authenticate/";
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

const apiFetchAllArtists = async () => {
  // TODO: This call can be removed once user login has been implemented
  var token = await apiAuthenticate(username, password);

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  // Call the API to get a list of all artists
  const url = baseUrl + "/artists/";
  const response = await fetch(url, {
    method: "GET",
    headers: headers,
  });

  // Get the response content as JSON and return it
  const artists = await response.json();
  return artists;
};

const apiFetchAlbumsByArtist = async (artistId) => {
  // TODO: This call can be removed once user login has been implemented
  var token = await apiAuthenticate(username, password);

  // Construct the request headers
  const headers = {
    Authorization: `Bearer ${token}`,
  };

  // Call the API to get a list of all artists
  const url = baseUrl + `/albums/artist/${artistId}`;
  const response = await fetch(url, {
    method: "GET",
    headers: headers,
  });

  // Get the response content as JSON and return it
  const albums = await response.json();
  return albums;
};

export { apiAuthenticate, apiFetchAllArtists, apiFetchAlbumsByArtist };
