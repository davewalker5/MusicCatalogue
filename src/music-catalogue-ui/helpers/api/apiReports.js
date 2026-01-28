import config from "@/config.json";
import { apiGetHeaders } from "./apiHeaders";
import { apiReadResponseData, apiFormatDateTime } from "./apiUtils";

/**
 * Call the API to retrieve the job status report
 * @param {*} from
 * @param {*} to
 * @param {*} logout
 * @returns
 */
const apiJobStatusReport = async (from, to, logout) => {
  // Make sure the dates cover the period 00:00:00 on the start date to 23:59:59 on the
  // end date
  const startDate = new Date(
    from.getFullYear(),
    from.getMonth(),
    from.getDate(),
    0,
    0,
    0
  );

  const endDate = new Date(
    to.getFullYear(),
    to.getMonth(),
    to.getDate(),
    23,
    59,
    59
  );

  // Construct the route. Dates need to be formatted in a specific fashion for the API
  // to decode them and they also need to be URL encoded
  const fromRouteSegment = apiFormatDateTime(startDate);
  const toRouteSegment = apiFormatDateTime(endDate);
  const url = `${config.api.baseUrl}/reports/jobs/${fromRouteSegment}/${toRouteSegment}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the genre statistics report
 * @param {*} wishlist
 * @param {*} logout
 * @returns
 */
const apiGenreStatisticsReport = async (wishlist, logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/genres/${wishlist}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the artist statistics report
 * @param {*} wishlist
 * @param {*} logout
 * @returns
 */
const apiArtistStatisticsReport = async (wishlist, logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/artists/${wishlist}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the monthly spending report
 * @param {*} logout
 * @returns
 */
const apiMonthlySpendReport = async (logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/spend/false`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the retailer statistics report
 * @param {*} wishlist
 * @param {*} logout
 * @returns
 */
const apiRetailerStatisticsReport = async (wishlist, logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/retailers/${wishlist}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the albums by genre report
 * @param {*} genreId
 * @param {*} logout
 */
const apiGenreAlbumsReport = async (genreId, logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/genreAlbums/${genreId}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

/**
 * Call the API to retrieve the albums by purchase date report
 * @param {*} year
 * @param {*} month
 * @param {*} logout
 */
const apiAlbumsByPurchaseDateReport = async (year, month, logout) => {
  // Construct the route
  const url = `${config.api.baseUrl}/reports/albumsByPurchaseDate/${year}/${month}`;

  // Call the API to get content for the report
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const records = await apiReadResponseData(response, logout);
  return records;
};

export {
  apiJobStatusReport,
  apiGenreStatisticsReport,
  apiArtistStatisticsReport,
  apiMonthlySpendReport,
  apiRetailerStatisticsReport,
  apiGenreAlbumsReport,
  apiAlbumsByPurchaseDateReport,
};
