import config from "../config.json";
import { apiGetPostHeaders } from "./apiHeaders";

/**
 * Request an export of the catalogue
 * @param {*} fileName
 * @param {*} logout
 */
const apiRequestCatalogueExport = async (fileName, logout) => {
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

/**
 * Request an export of the artist statistics report
 * @param {*} fileName
 * @param {*} isWishList
 * @param {*} logout
 */
const apiRequestAristStatisticsExport = async (
  fileName,
  isWishList,
  logout
) => {
  // Create a JSON body containing the file name to export to
  const body = JSON.stringify({
    fileName: fileName,
    wishList: isWishList,
  });

  // Call the API to request the export
  const url = `${config.api.baseUrl}/export/artiststatistics`;
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

export { apiRequestCatalogueExport, apiRequestAristStatisticsExport };
