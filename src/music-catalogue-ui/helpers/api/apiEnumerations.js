import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetHeaders } from "./apiHeaders";

/**
 * Fetch a list of vocal presence options from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const apiFetchVocalPresences = async (logout) => {
  // Call the API to get a list of options
  const url = `${config.api.baseUrl}/enumerations/vocalpresence/`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const options = await apiReadResponseData(response, logout);
  return options;
};

/**
 * Fetch a list of ensemble type options from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const apiFetchEnsembleTypes = async (logout) => {
  // Call the API to get a list of options
  const url = `${config.api.baseUrl}/enumerations/ensembletype/`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const options = await apiReadResponseData(response, logout);
  return options;
};

/**
 * Fetch a list of playlist type options from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const apiFetchPlaylistTypes = async (logout) => {
  // Call the API to get a list of options
  const url = `${config.api.baseUrl}/enumerations/playlisttype/`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const options = await apiReadResponseData(response, logout);
  return options;
};

/**
 * Fetch a list of time of day options from the Music Catalogue REST API
 * @param {*} logout
 * @returns
 */
const apiFetchTimesOfDay = async (logout) => {
  // Call the API to get a list of options
  const url = `${config.api.baseUrl}/enumerations/timeofday/`;
  const response = await fetch(url, {
    method: "GET",
    headers: apiGetHeaders(),
  });

  const options = await apiReadResponseData(response, logout);
  return options;
};

export { apiFetchVocalPresences, apiFetchEnsembleTypes, apiFetchPlaylistTypes, apiFetchTimesOfDay };
