import config from "../config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders } from "./apiHeaders";

/**
 * Create a retailer or return an existing retailer with the specified name
 * @param {*} retailer
 * @param {*} logout
 * @returns
 */
const apiCreateRetailer = async (retailer, logout) => {
  // Create the request body
  const body = JSON.stringify({ name: retailer });

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

export { apiCreateRetailer };
