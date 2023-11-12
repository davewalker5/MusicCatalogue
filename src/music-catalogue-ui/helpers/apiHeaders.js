import { apiGetToken } from "./apiToken";

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

export { apiGetHeaders, apiGetPostHeaders };
