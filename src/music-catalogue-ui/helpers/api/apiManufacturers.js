import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create an manufacturer
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiCreateManufacturer = async (name, logout) => {
  // Create the request body
  const body = JSON.stringify({
    name: name,
  });

  // Call the API to create the manufacturer. This will just return the current
  // record if it already exists
  const url = `${config.api.baseUrl}/manufacturers`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipmentType = await apiReadResponseData(response, logout);
  return equipmentType;
};

/**
 * Update an manufacturer
 * @param {*} id
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiUpdateManufacturer = async (id, name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: id,
    name: name,
  });

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/manufacturers`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipmentType = await apiReadResponseData(response, logout);
  return equipmentType;
};

/**
 * Delete the manufacturer with the specified ID
 * @param {*} equipmentTypeId
 * @param {*} logout
 * @returns
 */
const apiDeleteManufacturer = async (equipmentTypeId, logout) => {
  // Call the API to delete the specified manufacturer
  const url = `${config.api.baseUrl}/manufacturers/${equipmentTypeId}`;
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
 * Return a list of manufacturers
 * @param {*} logout
 * @returns
 */
const apiFetchManufacturers = async (logout) => {
  // Call the API to retrieve the manufacturer list
  const url = `${config.api.baseUrl}/manufacturers`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const manufacturers = await apiReadResponseData(response, logout);
  return manufacturers;
};

export {
  apiCreateManufacturer,
  apiUpdateManufacturer,
  apiDeleteManufacturer,
  apiFetchManufacturers,
};
