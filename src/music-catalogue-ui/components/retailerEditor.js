import styles from "./retailerEditor.module.css";
import pages from "../helpers/navigation";
import { useState, useCallback } from "react";
import { apiCreateRetailer, apiUpdateRetailer } from "@/helpers/apiRetailers";
import FormInputField from "./formInputField";
import { geocodeAddress } from "@/helpers/geocoder";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGlobe } from "@fortawesome/free-solid-svg-icons";

const RetailerEditor = ({ retailer, navigate, logout }) => {
  const [name, setName] = useState(retailer.name);
  const [address1, setAddress1] = useState(retailer.address1);
  const [address2, setAddress2] = useState(retailer.address2);
  const [town, setTown] = useState(retailer.town);
  const [county, setCounty] = useState(retailer.county);
  const [postCode, setPostCode] = useState(retailer.postCode);
  const [country, setCountry] = useState(retailer.country);
  const [latitude, setLatitude] = useState(retailer.latitude);
  const [longitude, setLongitude] = useState(retailer.longitude);
  const [webSite, setWebSite] = useState(retailer.webSite);
  const [error, setError] = useState("");

  // Callback to geocode the address
  const geocode = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Geocode the address
      const location = await geocodeAddress(
        address1,
        address2,
        town,
        county,
        postCode,
        country
      );

      // If we have an address, set the latitude and longitude
      if (location != null) {
        setLatitude(location.lat);
        setLongitude(location.lng);
      }
    },
    [address1, address2, town, county, postCode, country]
  );

  /* Callback to save retailer details */
  const saveRetailer = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors
      setError("");

      // Either add or update the retailer, depending on whether they currently
      // have an ID
      let updatedRetailer = null;
      if (retailer.id <= 0) {
        // Invalid ID, so create a new retailer
        updatedRetailer = await apiCreateRetailer(
          name,
          address1,
          address2,
          town,
          county,
          postCode,
          country,
          webSite,
          latitude,
          longitude,
          logout
        );
      } else {
        // Has a valid ID, so update an existing retailer
        updatedRetailer = await apiUpdateRetailer(
          retailer.id,
          name,
          address1,
          address2,
          town,
          county,
          postCode,
          country,
          webSite,
          latitude,
          longitude,
          logout
        );
      }

      // If all's well, display a confirmation message. Otherwise, show an error
      if (updatedRetailer == null) {
        const action = retailer.Id <= 0 ? "adding" : "updating";
        setError(`An error occurred ${action} the retailer`);
      } else {
        navigate({ page: pages.retailers });
      }
    },
    [
      retailer,
      latitude,
      longitude,
      name,
      address1,
      address2,
      town,
      county,
      postCode,
      country,
      webSite,
      navigate,
      logout,
    ]
  );

  const title =
    retailer.town != null
      ? `${retailer.name} - ${retailer.town}`
      : retailer.name;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      <div className={styles.retailerEditorFormContainer}>
        <form className={styles.retailerEditorForm}>
          <div className="row">
            {error != "" ? (
              <div className={styles.retailerEditorError}>{error}</div>
            ) : (
              <></>
            )}
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Name"
                name="name"
                value={name}
                setValue={setName}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Address Line 1"
                name="address1"
                value={address1}
                setValue={setAddress1}
              />
            </div>
            <div className="col">
              <FormInputField
                label="Address Line 2"
                name="address2"
                value={address2}
                setValue={setAddress2}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Town"
                name="town"
                value={town}
                setValue={setTown}
              />
            </div>
            <div className="col">
              <FormInputField
                label="County"
                name="county"
                value={county}
                setValue={setCounty}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <div className="form-group mt-3">
                <label className={styles.retailerEditorInputLabel}>
                  Postcode
                </label>
                <FontAwesomeIcon
                  icon={faGlobe}
                  className={styles.retailerEditorGeocode}
                  onClick={(e) => geocode(e)}
                />
                <input
                  className="form-control"
                  placeholder="Postcode"
                  name="postcode"
                  value={postCode}
                  onChange={(e) => setPostCode(e.target.value)}
                />
              </div>
            </div>
            <div className="col">
              <FormInputField
                label="Country"
                name="country"
                value={country}
                setValue={setCountry}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Latitude"
                name="latitude"
                value={latitude}
                setValue={setLatitude}
              />
            </div>
            <div className="col">
              <FormInputField
                label="Longitude"
                name="longitude"
                value={longitude}
                setValue={setLongitude}
              />
            </div>
          </div>
          <div className="row align-items-start">
            <div className="col">
              <FormInputField
                label="Web Site"
                name="webSite"
                value={webSite}
                setValue={setWebSite}
              />
            </div>
          </div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className="d-grid gap-2 mt-3"></div>
          <div className={styles.retailerEditorButton}>
            <button
              className="btn btn-primary"
              onClick={(e) => saveRetailer(e)}
            >
              Save
            </button>
          </div>
          <div className={styles.retailerEditorButton}>
            <button
              className="btn btn-primary"
              onClick={() =>
                navigate({
                  page: pages.retailers,
                })
              }
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default RetailerEditor;
