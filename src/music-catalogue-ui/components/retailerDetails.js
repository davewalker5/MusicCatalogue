import { Parser } from "html-to-react";
import styles from "./retailerDetails.module.css";

const RetailerDetails = ({ retailer }) => {
  const haveCoordinates =
    retailer.latitude != null && retailer.longitude != null;
  const position = `${retailer.latitude},${retailer.longitude}`;
  const title = `${retailer.name} - ${retailer.town}`;
  const mapId = title.replace(/\-/g, "").replace(/ /g, "_");
  const mapHtml = `
  <gmp-map center="${position}" zoom="15" map-id="${mapId}">
    <gmp-advanced-marker position="${position}" title="${title}">
    </gmp-advanced-marker>
  </gmp-map>`;

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">{title}</h5>
      </div>
      {haveCoordinates == true ? (
        <div className={styles.retailerDetailsMapContainer}>
          {Parser().parse(mapHtml)}
        </div>
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
