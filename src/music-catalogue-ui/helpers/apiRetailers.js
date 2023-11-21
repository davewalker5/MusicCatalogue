import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create a retailer or return an existing retailer with the specified name
 * @param {*} retailerName
 * @param {*} logout
 * @returns
 */
const apiCreateRetailer = async (retailerName, logout) => {
  // Create the request body
  const body = JSON.stringify({ name: retailerName });

  // Call the API to create the retailer. This will just return the current
  // record if it already exists
  const url = `${config.api.baseUrl}/retailers`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const retailer = await apiReadResponseData(response, logout);
  return retailer;
};

/**
 * Return a list of retailer details
 * @param {*} logout
 * @returns
 */
const apiFetchRetailers = async (logout) => {
  // Call the API to retrieve the retailer list
  const url = `${config.api.baseUrl}/retailers`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const retailers = await apiReadResponseData(response, logout);
  return retailers;
};

/**
 * Update a retailer's details
 * @param {*} id
 * @param {*} address1
 * @param {*} address2
 * @param {*} town
 * @param {*} county
 * @param {*} postCode
 * @param {*} country
 * @param {*} webSite
 * @param {*} logout
 * @returns
 */
const apiUpdateRetailer = async (
  id,
  name,
  address1,
  address2,
  town,
  county,
  postCode,
  country,
  webSite,
  latitude,
  longitude,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    id: id,
    name: name,
    address1: address1,
    address2: address2,
    town: town,
    county: county,
    postCode: postCode,
    country: country,
    webSite: webSite,
    latitude: latitude,
    longitude: longitude,
  });
  console.log(body);

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/retailers`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const updatedRetailer = await apiReadResponseData(response, logout);
  return updatedRetailer;
};

export { apiCreateRetailer, apiFetchRetailers, apiUpdateRetailer };
