/**
 * Format a date/time in a manner suitable for use with the API without using
 * 3rd party libraries
 * @param {F} date
 */
const apiFormatDateTime = (date) => {
  // Get the formatted date components
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");

  // Get the formatted time components
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");
  const seconds = date.getSeconds().toString().padStart(2, "0");

  // Construct and return the formatted date/time
  const formattedDate =
    year +
    "-" +
    month +
    "-" +
    day +
    "%20" +
    hours +
    ":" +
    minutes +
    ":" +
    seconds;
  return formattedDate;
};

/**
 * Read the response content as JSON and return the resulting object
 * @param {*} response
 * @returns
 */
const apiReadResponseData = async (response, logout) => {
  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else if (response.status == 204) {
    // If the response is "no content", return NULL
    return null;
  } else if (response.ok) {
    // Get the response content as JSON and return it
    const data = await response.json();
    return data;
  } else {
    return null;
  }
};

export { apiFormatDateTime, apiReadResponseData };
