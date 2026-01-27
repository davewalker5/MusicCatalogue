import secrets from "@/helpers/secrets";
import config from "@/config.json";
import { setDefaults, geocode, RequestType } from "react-geocode";
import { getStorageValue } from "./storage";

const geocodeAddress = async (
  address1,
  address2,
  town,
  county,
  postcode,
  country
) => {
  // This sets default values for language and region for geocoding requests.
  const apiKey = getStorageValue(secrets.mapsApiKey);
  setDefaults({
    key: apiKey,
    language: config.region.geocodingLanguage,
    region: config.region.geocodingRegion,
  });

  // Construct an address from its components
  const address = [
    address1,
    address1,
    address2,
    town,
    county,
    postcode,
    country,
  ].join(",");

  try {
    // Geocode the address
    const response = await geocode(RequestType.ADDRESS, address);
    const location = response.results[0].geometry.location;
    return location;
  } catch {
    return null;
  }
};

export { geocodeAddress };
