import pages from "../helpers/navigation";
import LocationMap from "./locationMap";
import styles from "./retailerDetails.module.css";

/**
 * Component to show retailer addressing and web site details and map location
 * @param {*} param0
 * @returns
 */
const RetailerDetails = ({ mapsApiKey, retailer, navigate, logout }) => {
  // Set a flag indicating if we have enough data to show the map
  const canShowMap = retailer.latitude != null && retailer.longitude != null;

  const title =
    retailer.town != null
      ? `${retailer.name} - ${retailer.town}`
      : retailer.name;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      {canShowMap == true ? (
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
              <td>{retailer.postCode}</td>
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
        <div className={styles.retailerDetailsButtonContainer}>
          <button
            className="btn btn-primary"
            onClick={() =>
              navigate({ page: pages.retailerEditor, retailer: retailer })
            }
          >
            Edit
          </button>
        </div>
      </div>
    </>
  );
};

export default RetailerDetails;
