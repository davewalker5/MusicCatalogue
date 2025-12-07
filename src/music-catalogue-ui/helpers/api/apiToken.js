import {
  setStorageValue,
  getStorageValue,
  clearStorageValue,
} from "../storage";

/**
 * Store the JWT token
 * @param {*} token
 */
const apiSetToken = (token) => setStorageValue("token", token);

/**
 * Retrieve the current JWT token
 * @param {*} token
 * @returns
 */
const apiGetToken = () => getStorageValue("token");

/**
 * Clear the current JWT token
 */
const apiClearToken = () => clearStorageValue("token");

export { apiClearToken, apiSetToken, apiGetToken };
