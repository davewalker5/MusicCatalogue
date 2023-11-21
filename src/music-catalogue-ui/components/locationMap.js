import GoogleMapReact from "google-map-react";
import { useState } from "react";
import styles from "./locationMap.module.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMapMarkerAlt } from "@fortawesome/free-solid-svg-icons";

const LocationMap = ({ apiKey, latitude, longitude }) => {
  const [location, setLocation] = useState({ lat: latitude, lng: longitude });

  return (
    <>
      <div className={styles.locationMapContainer}>
        <GoogleMapReact
          bootstrapURLKeys={{ key: apiKey }}
          defaultCenter={location}
          defaultZoom={15}
        >
          <FontAwesomeIcon icon={faMapMarkerAlt} size="3x" color="red" />
        </GoogleMapReact>
      </div>
    </>
  );
};

export default LocationMap;
