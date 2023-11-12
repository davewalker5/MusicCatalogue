import config from "../config.json";

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

export { apiAuthenticate };
