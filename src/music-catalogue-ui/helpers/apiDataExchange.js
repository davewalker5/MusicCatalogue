import config from "../config.json";
import { apiGetPostHeaders } from "./apiHeaders";

/**
 * Request an export of the catalogue
 * @param {*} fileName
 * @param {*} logout
 */
const apiRequestExport = async (fileName, logout) => {
  // Create a JSON body containing the file name to export to
  const body = JSON.stringify({
    fileName: fileName,
  });

  // Call the API to request the export
  const url = `${config.api.baseUrl}/export/catalogue`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  }

  return response.ok;
};

export { apiRequestExport };
