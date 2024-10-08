import React, { useCallback, useState } from "react";
import styles from "./reports.module.css";
import { apiGenreAlbumsReport } from "@/helpers/api/apiReports";
import ReportExportControls from "./reportExportControls";
import { apiRequestGenreAlbumsExport } from "@/helpers/api/apiDataExchange";
import GenreSelector from "../genres/genreSelector";
import AlbumsTable from "./albumsTable";

/**
 * Component to display the albums by genre report page and its results
 * @param {*} logout
 * @returns
 */
const GenreAlbumsReport = ({ logout }) => {
  const [genre, setGenre] = useState(null);
  const [records, setRecords] = useState(null);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Callback to request the albums by genre report from the API
  const getReportCallback = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // If there's a genre selected, request the report
      if (genre != null) {
        const fetchedRecords = await apiGenreAlbumsReport(genre.id, logout);
        setRecords(fetchedRecords);
      }
    },
    [genre, logout]
  );

  /* Callback to export the report */
  const exportReportCallback = useCallback(
    async (e, fileName) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Clear pre-existing errors and messages
      setMessage("");
      setError("");

      // Set the genre from the drop-down selection
      const genreId = genre != null ? genre.id : null;

      // Request an export via the API
      const isOK = await apiRequestGenreAlbumsExport(fileName, genreId, logout);

      // If all's well, display a confirmation message. Otherwise, show an error
      if (isOK) {
        setMessage(
          `A background export of the albums by genre report to ${fileName} has been requested`
        );
      } else {
        setError(
          "An error occurred requesting an export of the albums by genre report"
        );
      }
    },
    [genre, logout]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">Albums by Genre Report</h5>
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
                  <label className={styles.reportFormLabel}>Report For:</label>
                </div>
                <div className="col">
                  <div className={styles.reportGenreSelector}>
                    <GenreSelector
                      initialGenre={genre}
                      genreChangedCallback={setGenre}
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

export default GenreAlbumsReport;
