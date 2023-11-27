import GoogleMapReact from "google-map-react";
import styles from "./locationMap.module.css";
import secrets from "@/helpers/secrets";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMapMarkerAlt } from "@fortawesome/free-solid-svg-icons";
import { getStorageValue } from "@/helpers/storage";

/**
 * Component to render a location map, with pin at the specified coordinates
 * @param {*} latitude
 * @param {*} longitude
 * @returns
 */
const LocationMap = ({ latitude, longitude }) => {
  const location = { lat: latitude, lng: longitude };
  const mapsApiKey = getStorageValue(secrets.mapsApiKey);

  return (
    <>
      <div className={styles.locationMapContainer}>
        <GoogleMapReact
          bootstrapURLKeys={{ key: mapsApiKey }}
          defaultCenter={location}
          defaultZoom={15}
        >
          <FontAwesomeIcon
            lat={latitude}
            lng={longitude}
            icon={faMapMarkerAlt}
            size="3x"
            color="red"
          />
        </GoogleMapReact>
      </div>
    </>
  );
};

export default LocationMap;
