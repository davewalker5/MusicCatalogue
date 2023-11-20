import GoogleMapReact from "google-map-react";
import { useState } from "react";
import styles from "./locationMap.module.css";

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
          <div className={styles.locationMapMarker}></div>
        </GoogleMapReact>
      </div>
    </>
  );
};

export default LocationMap;