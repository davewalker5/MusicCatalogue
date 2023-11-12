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

export { apiClearToken, apiSetToken, apiGetToken };
