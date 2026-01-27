import React, { useCallback, useState } from "react";
import styles from "./reports.module.css";
import DatePicker from "react-datepicker";
import { apiAlbumsByPurchaseDateReport } from "@/helpers/api/apiReports";
import ReportExportControls from "./reportExportControls";
import { apiRequestAlbumsByPurchaseDateExport } from "@/helpers/api/apiDataExchange";
import AlbumsTable from "./albumsTable";

/**
 * Component to display the albums by genre report page and its results
 * @param {*} logout
 * @returns
 */
const AlbumsByPurchaseDateReport = ({ logout }) => {
  const [purchaseDate, setPurchaseDate] = useState(null);
  const [records, setRecords] = useState(null);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request the albums by purchase date report from the API
  const getReportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // If there's a date selected, request the report
      if (purchaseDate != null) {
        const year = purchaseDate.getFullYear();
        const month = 1 + purchaseDate.getMonth();
        const fetchedRecords = await apiAlbumsByPurchaseDateReport(
          year,
          month,
          logout
        );
        setRecords(fetchedRecords);
      }
    },
    [purchaseDate, logout]
  );

  /* Callback to export the report */
  const exportReportCallback = useCallback(
    async (e, fileName) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // Request an export via the API
      const year = purchaseDate.getFullYear();
      const month = 1 + purchaseDate.getMonth();
      const isOK = await apiRequestAlbumsByPurchaseDateExport(
        fileName,
        year,
        month,
        logout
      );

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the albums by purchase date report to ${fileName} has been requested`
        );
      } else {
        setError(
          "An error occurred requesting an export of the albums by purchase date report"
        );
      }
    },
    [purchaseDate, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">
          Albums by Purchase Date Report
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
            <div className="mt-3">
              <div className="d-inline-flex align-items-center">
                <div className="col">
                  <label className={styles.reportFormLabel}>
                    Purchase Date:
                  </label>
                </div>
                <div className="col">
                  <div>
                    <DatePicker
                      dateFormat="MMMM yyyy"
                      showMonthYearPicker
                      selected={purchaseDate}
                      onChange={(date) => setPurchaseDate(date)}
                    />
                  </div>
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
      <AlbumsTable records={records} />
    </>
  );
};

export default AlbumsByPurchaseDateReport;
