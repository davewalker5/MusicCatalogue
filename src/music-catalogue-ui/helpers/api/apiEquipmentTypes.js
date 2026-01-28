import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create an equipment type
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiCreateEquipmentType = async (name, logout) => {
  // Create the request body
  const body = JSON.stringify({
    name: name,
  });

  // Call the API to create the equipment type. This will just return the current
  // record if it already exists
  const url = `${config.api.baseUrl}/equipmenttypes`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipmentType = await apiReadResponseData(response, logout);
  return equipmentType;
};

/**
 * Update an equipment type
 * @param {*} id
 * @param {*} name
 * @param {*} logout
 * @returns
 */
const apiUpdateEquipmentType = async (id, name, logout) => {
  // Construct the body
  const body = JSON.stringify({
    id: id,
    name: name,
  });

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/equipmenttypes`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipmentType = await apiReadResponseData(response, logout);
  return equipmentType;
};

/**
 * Delete the equipment type with the specified ID
 * @param {*} equipmentTypeId
 * @param {*} logout
 * @returns
 */
const apiDeleteEquipmentType = async (equipmentTypeId, logout) => {
  // Call the API to delete the specified equipment type
  const url = `${config.api.baseUrl}/equipmenttypes/${equipmentTypeId}`;
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
 * Return a list of equipment types
 * @param {*} logout
 * @returns
 */
const apiFetchEquipmentTypes = async (logout) => {
  // Call the API to retrieve the equipment type list
  const url = `${config.api.baseUrl}/equipmenttypes`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const equipmentTypes = await apiReadResponseData(response, logout);
  return equipmentTypes;
};

export {
  apiCreateEquipmentType,
  apiUpdateEquipmentType,
  apiDeleteEquipmentType,
  apiFetchEquipmentTypes,
};
