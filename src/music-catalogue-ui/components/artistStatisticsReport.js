import React, { useCallback, useState } from "react";
import styles from "./reports.module.css";
import Select from "react-select";
import "react-datepicker/dist/react-datepicker.css";
import { apiArtistStatisticsReport } from "@/helpers/apiReports";
import ArtistStatisticsRow from "./artistStatisticsRow";
import ReportExportControls from "./reportExportControls";
import { apiRequestAristStatisticsExport } from "@/helpers/apiDataExchange";

/**
 * Component to display the artist statistics report page and its results
 * @param {*} logout
 * @returns
 */
const ArtistStatisticsReport = ({ logout }) => {
  const [catalogue, setCatalogue] = useState(0);
  const [records, setRecords] = useState(null);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request the artist statistics report from the API
  const getReportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Set the wishlist flag from the drop-down selection
      const forWishList = catalogue.value == "wishlist";

      // Fetch the report
      const fetchedRecords = await apiArtistStatisticsReport(
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
      const isOK = await apiRequestAristStatisticsExport(
        fileName,
        forWishList,
        logout
      );

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the artist statistics report to ${fileName} has been requested`
        );
      } else {
        setError(
          "An error occurred requesting an export of the artist statistics report"
        );
      }
    },
    [catalogue, logout]
  );

  // Construct a list of select list options for the directory
  const options = [
    { value: "catalogue", label: "Main Catalogue" },
    { value: "wishlist", label: "Wish List" },
  ];

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Artist Statistics Report</h5>
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
                  <Select
                    className={styles.reportCatalogueSelector}
                    defaultValue={catalogue}
                    onChange={setCatalogue}
                    options={options}
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
            <th>Albums</th>
            <th>Tracks</th>
            <th>Total Spend</th>
          </tr>
        </thead>
        {records != null && (
          <tbody>
            {records.map((r) => (
              <ArtistStatisticsRow key={r.id} record={r} />
            ))}
          </tbody>
        )}
      </table>
    </>
  );
};

export default ArtistStatisticsReport;
