import React, { useCallback, useState } from "react";
import styles from "./reports.module.css";
import catalogues from "@/helpers/catalogues";
import "react-datepicker/dist/react-datepicker.css";
import { apiRetailerStatisticsReport } from "@/helpers/apiReports";
import RetailerStatisticsRow from "./retailerStatisticsRow";
import ReportExportControls from "./reportExportControls";
import { apiRequestRetailerStatisticsExport } from "@/helpers/apiDataExchange";
import CatalogueSelector from "./catalogueSelector";

/**
 * Component to display the retailer statistics report page and its results
 * @param {*} logout
 * @returns
 */
const RetailerStatisticsReport = ({ logout }) => {
  // Set up state
  const [catalogue, setCatalogue] = useState(catalogues.main);
  const [records, setRecords] = useState(null);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request the retailer statistics report from the API
  const getReportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Set the wishlist flag from the drop-down selection
      const forWishList = catalogue == catalogues.wishlist;

      // Fetch the report
      const fetchedRecords = await apiRetailerStatisticsReport(
        forWishList,
        logout
      );
      setRecords(fetchedRecords);
    },
    [catalogue, logout]
  );

  /* Callback to export the report */
  const exportReportCallback = useCallback(
    async (e, fileName) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // Set the wishlist flag from the drop-down selection
      const forWishList = catalogue.value == "wishlist";

      // Request an export via the API
      const isOK = await apiRequestRetailerStatisticsExport(
        fileName,
        forWishList,
        logout
      );

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the retailer statistics report to ${fileName} has been requested`
        );
      } else {
        setError(
          "An error occurred requesting an export of the retailer statistics report"
        );
      }
    },
    [catalogue, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">
          Retailer Statistics Report
        </h5>
      </div>
      <div className={styles.reportFormContainer}>
        <form className={styles.reportForm}>
          {message != "" ? (
            <div className={styles.reportExportMessage}>{message}</div>
          ) : (
            <></>
          )}
          {error != "" ? (
            <div className={styles.reportExportError}>{error}</div>
          ) : (
            <></>
          )}
          <div className="row" align="center">
            <div className="mt-6">
              <div className="d-inline-flex align-items-center">
                <div className="col">
                  <label className={styles.reportFormLabel}>Report For:</label>
                </div>
                <div className="col">
                  <CatalogueSelector
                    selectedCatalogue={catalogues.main}
                    catalogueChangedCallback={setCatalogue}
                  />
                </div>
                <div className="col">
                  <button
                    className="btn btn-primary"
                    onClick={(e) => getReportCallback(e)}
                  >
                    Search
                  </button>
                </div>
                {records != null ? (
                  <ReportExportControls
                    isPrimaryButton={false}
                    exportReport={exportReportCallback}
                  />
                ) : (
                  <></>
                )}
              </div>
            </div>
          </div>
        </form>
      </div>
      <table className="table table-hover">
        <thead>
          <tr>
            <th>Name</th>
            <th>Artists</th>
            <th>Albums</th>
            <th>Tracks</th>
            <th>Total Spend</th>
          </tr>
        </thead>
        {records != null && (
          <tbody>
            {records.map((r) => (
              <RetailerStatisticsRow key={r.id} record={r} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default RetailerStatisticsReport;
