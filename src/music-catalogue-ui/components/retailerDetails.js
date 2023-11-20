import LocationMap from "./locationMap";
import styles from "./retailerDetails.module.css";

/**
 * Component to show retailer addressing and web site details and map location
 * @param {*} param0
 * @returns
 */
const RetailerDetails = ({ mapsApiKey, retailer, logout }) => {
  // Create map properties
  const haveCoordinates =
    retailer.latitude != null && retailer.longitude != null;
  const title = `${retailer.name} - ${retailer.town}`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      {haveCoordinates == true ? (
        <LocationMap
          apiKey={mapsApiKey}
          latitude={retailer.latitude}
          longitude={retailer.longitude}
          logout={logout}
        />
      ) : (
        <></>
      )}
      <div className={styles.retailerDetailsContainer}>
        <table className={styles.retailerDetailsTable}>
          <tbody>
            <tr>
              <th>Address:</th>
              <td>{retailer.address1}</td>
            </tr>
            {retailer.address2 != null ? (
              <tr>
                <th>Address:</th>
                <td>{retailer.address2}</td>
              </tr>
            ) : (
              <></>
            )}
            <tr>
              <th>Town:</th>
              <td>{retailer.town}</td>
            </tr>
            <tr>
              <th>County:</th>
              <td>{retailer.county}</td>
            </tr>
            <tr>
              <th>Postcode:</th>
              <td>{retailer.postcode}</td>
            </tr>
            <tr>
              <th>Country:</th>
              <td>{retailer.country}</td>
            </tr>
            {retailer.webSite != null ? (
              <tr>
                <th>Web Site:</th>
                <td>
                  <a href={retailer.webSite} target="_blank">
                    {retailer.webSite}
                  </a>
                </td>
              </tr>
            ) : (
              <></>
            )}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default RetailerDetails;
